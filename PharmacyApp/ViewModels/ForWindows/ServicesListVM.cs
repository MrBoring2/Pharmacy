using PharmacyApp.Helpers;
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
    public class ServicesListVM : BaseWindowVM
    {
        private bool? dialogResult;
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
        private string seartchText;
        private LaboratoryService selectedService;
        private Filter searchParameter;
        private RelayCommand select;
        public List<Filter> FilterParameters { get; set; }
        private ObservableCollection<LaboratoryService> LaboratoryServices { get; set; }
        private RestService restService;


        public ServicesListVM()
        {
            restService = new RestService();
            FilterParameters = new List<Filter>()
            {
                new Filter("Название", "Name"),
                new Filter("Код", "Code"),
                new Filter("Стоимость", "Price"),
            };

            LoadCompanies();
            SearchText = string.Empty;
        }


        private void LoadCompanies()
        {
            RestRequest request = new RestRequest($"{Constants.apiAddress}/api/LaboratoryServices", Method.GET);
            var response = restService.SendRequest(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<List<LaboratoryService>>(response.Content);
                LaboratoryServices = new ObservableCollection<LaboratoryService>(data);
            }
        }

        public string SearchText
        {
            get => seartchText;
            set
            {
                seartchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterLaboratoryServices));
            }
        }

        public LaboratoryService SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                OnPropertyChanged();


            }
        }

        public Filter SearchParameter
        {
            get => searchParameter;
            set
            {
                searchParameter = value;
                OnPropertyChanged();
                SearchText = string.Empty;
                OnPropertyChanged(nameof(FilterLaboratoryServices));
            }
        }
        public ObservableCollection<LaboratoryService> FilterLaboratoryServices
        {
            get => SearchParameter != null && SearchParameter.Property != string.Empty && SearchText != string.Empty ?
                new ObservableCollection<LaboratoryService>(LaboratoryServices.Where(p => SearchParameter.IsEqual(p, SearchText)))
                : new ObservableCollection<LaboratoryService>(LaboratoryServices);
        }
        public RelayCommand AddPatient { get; set; }
        public RelayCommand Select
        {
            get
            {
                return select ?? (select = new RelayCommand(obj =>
                {
                    if (SelectedService != null)
                    {
                        DialogResult = true;
                    }
                    else Notification.ShowNotification("Услуга не выбрана!", "Внимание", NotificationType.Warning);
                }));
            }
        }
        private RelayCommand cancel;
        public RelayCommand Cancel
        {
            get
            {
                return cancel ?? (cancel = new RelayCommand(obj =>
                {
                    DialogResult = false;
                }));
            }
        }
    }
}
