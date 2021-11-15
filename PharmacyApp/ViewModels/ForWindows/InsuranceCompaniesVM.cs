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
    public class InsuranceCompaniesVM : BaseWindowVM
    {
        private bool? dialogResult;
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
        private RestService restService;
        private ObservableCollection<InsuranceCompany> insuranceCompanies;
        public ObservableCollection<InsuranceCompany> InsuranceCompanies { get => insuranceCompanies; set { insuranceCompanies = value; OnPropertyChanged(); } }
        private InsuranceCompany selectedCompany;
        public InsuranceCompany SelectedCompany { get => selectedCompany; set { selectedCompany = value; OnPropertyChanged(); } }

        public RelayCommand Select { get; set; }
        public InsuranceCompaniesVM()
        {
            restService = new RestService();
            LoadCompanies();
            Select = new RelayCommand(SelectCompany);

        }

        private void LoadCompanies()
        {
            var request = new RestRequest("api/InsuranceСompanies", Method.GET);
            var response = restService.SendRequest(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                InsuranceCompanies = new ObservableCollection<InsuranceCompany>(JsonSerializer.Deserialize<List<InsuranceCompany>>(response.Content));
            }
        }

        private void SelectCompany(object obj)
        {
            DialogResult = true;
        }
    }
}
