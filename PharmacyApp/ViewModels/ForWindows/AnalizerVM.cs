using PharmacyApp.Helpers;
using PharmacyApp.Models.ListViewModels;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class AnalizerVM : BaseWindowVM
    {
        private Timer timer = new Timer(100);
        private string analizerURL = "http://localhost:5000/api/analyzer/";
        public AnalizerView Analizer { get; set; }
        private RestService restService;

        private NotCompleteService selectedService;
        public NotCompleteService SelectedService { get => selectedService; set { selectedService = value; OnPropertyChanged(); } }
        public AnalizerVM(AnalizerView analizer)
        {
            Analizer = analizer;
            restService = new RestService();
            LoadServices();
            Send = new RelayCommand(SendService);
        }


        private void SendService(object obj)
        {
            var client = new RestClient(analizerURL);
            AnalizerPostModel analizerPostModel = new AnalizerPostModel();
            analizerPostModel.patientId = SelectedService.Order.PatientId.ToString();
            analizerPostModel.services = new List<ServiceModel>();
            analizerPostModel.services.Add(new ServiceModel { serviceCode = SelectedService.LaboratoryService.Code });
            var a = JsonSerializer.Serialize(analizerPostModel);
            var request = new RestRequest(Analizer.AnalizerName, Method.POST).AddJsonBody(JsonSerializer.Serialize(analizerPostModel));
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var changedService = NotComleteServices.FirstOrDefault(p => p.LaboratoryService.Code.Equals(SelectedService.LaboratoryService.Code));
                if (changedService != null)
                {
                    changedService.Status = "On research";
                    changedService.IsRunning = true;
                    Analizer.IsBuzy = true;
                    OnPropertyChanged(nameof(NotComleteServices));
                }
                timer.Elapsed += async (sender, e) => await UpdateProgress(client);
                timer.Start();
            }
        }

        private Task UpdateProgress(RestClient client)
        {
            return Task.Run(async () =>
            {
                var requestGET = new RestRequest(Analizer.AnalizerName, Method.GET);
                var responseGET = client.Execute(requestGET);
                if (responseGET.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (responseGET.Content.Contains("result"))
                    {
                   
                        var data = JsonSerializer.Deserialize<AnalizerResearchFinish>(responseGET.Content);
                        var changedService = NotComleteServices.FirstOrDefault(p => p.LaboratoryService.Code.Equals(data.services[0].serviceCode));
                        if (changedService != null)
                        {
                            changedService.IsRunning = false;
                            changedService.Progress = 100;
                            changedService.Result = data.services[0].result.ToString();
                            Analizer.IsBuzy = false;

                           // OnPropertyChanged(nameof(NotComleteServices));
                            App.Current.Dispatcher.Invoke(()=> UpdateServices());

                            var resultDialogVM = new AnalizerResultVM(data.services[0].result.ToString());
                            await Task.Run(() => { WindowNavigation.Instance.OpenModalWindow(resultDialogVM); });
                            if(resultDialogVM.DialogResult == true)
                            {
                                changedService.Status = "Finished";

                                var serviceOrder = new LaboratoryServicesToOrder
                                {
                                    Accepted = true,
                                    AnalyzerId = Analizer.AnalizerId,
                                    DateOfFinished = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, TimeSpan.Zero).ToUnixTimeMilliseconds().ToString(),
                                    LaboratoryServiceId = changedService.LaboratoryService.Code,
                                    OrderId = changedService.Order.Id,
                                    Result = changedService.Result,
                                    Status = changedService.Status,
                                    UserId = UserService.Instance.UserId                                 
                                };

                                App.Current.Dispatcher.Invoke(() => UpdateServices());
                                var b = JsonSerializer.Serialize(serviceOrder);
                                var updateServiceRequest = new RestRequest($"api/LaboratoryServicesToOrders/{changedService.LaboratoryService.Code}"
                                    , Method.PUT)
                                    .AddJsonBody(JsonSerializer.Serialize(serviceOrder));
                                var response = restService.SendRequest(updateServiceRequest);
                                timer.Stop();
                                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                                {
                                    MessageBox.Show("Исследование завершено!");
                                }

                            }
                            else
                            {
                                changedService.Status = "Rejected";


                                var serviceOrder = new LaboratoryServicesToOrder
                                {
                                    Accepted = false,
                                    AnalyzerId = Analizer.AnalizerId,
                                    DateOfFinished = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, TimeSpan.Zero).ToUnixTimeMilliseconds().ToString(),
                                    LaboratoryServiceId = changedService.LaboratoryService.Code,
                                    OrderId = changedService.Order.Id,
                                    Result = changedService.Result,
                                    Status = changedService.Status,
                                    UserId = UserService.Instance.UserId,
                                };
                                App.Current.Dispatcher.Invoke(() => UpdateServices());
                                var updateServiceRequest = new RestRequest($"api/LaboratoryServicesToOrders/{changedService.LaboratoryService.Code}"
                                    , Method.PUT)
                                    .AddJsonBody(JsonSerializer.Serialize(serviceOrder));
                                var response = restService.SendRequest(updateServiceRequest);
                                timer.Stop();
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    MessageBox.Show("Исследование отклонено!");
                                }


                            }
                            OnPropertyChanged("NotComleteServices");

                        }
                    }
                    else
                    {
                        var data = JsonSerializer.Deserialize<AnalizerProgress>(responseGET.Content);
                        var changedService = NotComleteServices.FirstOrDefault(p => p.LaboratoryService.Code.Equals(SelectedService.LaboratoryService.Code));
                        if (changedService != null)
                        {
                            changedService.Progress = data.progress;
                        }
                        OnPropertyChanged(nameof(NotComleteServices));

                    }
                }
                else timer.Stop();
            });
        }

        private void UpdateServices()
        {
            var services = new ObservableCollection<NotCompleteService>(NotComleteServices);
            NotComleteServices.Clear();
            foreach (var item in services)
            {
                if(item.Status != "Finished" && item.Status != "Rejected")
                    NotComleteServices.Add(item);
            }
        }

        private string title;
        public string Title { get => title; set { title = value; OnPropertyChanged(); } }

        private ObservableCollection<NotCompleteService> notComleteServices;
        public ObservableCollection<NotCompleteService> NotComleteServices { get => notComleteServices; set { notComleteServices = value; OnPropertyChanged(); } }

        private void LoadServices()
        {
            NotComleteServices = new ObservableCollection<NotCompleteService>();
            var services = new List<LaboratoryServicesToOrder>();
            var request = new RestRequest("api/LaboratoryServicesToOrders/notCompleted", Method.GET);
            var response = restService.SendRequest(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                services = JsonSerializer.Deserialize<List<LaboratoryServicesToOrder>>(response.Content);

                foreach (var item in services)
                {
                    NotComleteServices.Add(new NotCompleteService { Order = item.Order, LaboratoryService = item.LaboratoryService, Status = "Not Complete", IsRunning = true, Progress = 0, Result = "Нет результата" });
                }
            }
        }


        public RelayCommand Send { get; set; }

    }
}
