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

namespace PharmacyApp.ViewModels.ForWindows
{
    public class AnalizerVM : BaseWindowVM
    {
        public AnalizerView Analizer { get; set; }
        private RestService restService;
        public AnalizerVM(AnalizerView analizer)
        {
            Analizer = analizer;
            restService = new RestService();
            LoadServices();
            Send = new RelayCommand(SendService);
            Accept = new RelayCommand(AcceptResult);
            Reject = new RelayCommand(RejectResult);
        }

        private void RejectResult(object obj)
        {
            throw new NotImplementedException();
        }

        private void AcceptResult(object obj)
        {
            throw new NotImplementedException();
        }

        private void SendService(object obj)
        {
            throw new NotImplementedException();
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
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                services = JsonSerializer.Deserialize<List<LaboratoryServicesToOrder>>(response.Content);

                foreach (var item in services)
                {
                    NotComleteServices.Add(new NotCompleteService { OrderId=item.OrderId, LaboratoryService = item.LaboratoryService, Status = "Not Complete", IsRunning = true, Progress = 0, Result = 0 });
                }
            }
        }


        public RelayCommand Send { get;set;}
        public RelayCommand Accept { get; set; }
        public RelayCommand Reject { get; set; }

    }
}
