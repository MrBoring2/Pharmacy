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
    public class PatientsListVM : BaseWindowVM
    {
        private string seartchText;
        private Patient selectedPatient;
        private Filter searchParameter;
        private RelayCommand select;
        public List<Filter> FilterParameters { get; set; }
        private ObservableCollection<Patient> Patients { get; set; }
        private RestService restService;


        public PatientsListVM()
        {
            restService = new RestService();
            FilterParameters = new List<Filter>()
            {
                new Filter("ФИО", "FullName"),
                new Filter("Логин", "Login"),
                new Filter("GUID", "Guid"),
                new Filter("Email", "Email"),
                new Filter("Номер страхового полиса", "SosialSecNumber"),
                new Filter("ИНН", "Ein"),
                new Filter("Тип страхового полиса", "SosialType"),
                new Filter("Телефон", "Telephone"),
                new Filter("Серия паспорта", "PassportSeries"),
                new Filter("Номер паспорта", "PassportNumber"),
                new Filter("Дата рождения", "DateOfBirth")           
            };

            LoadPatients();
        }

        public PatientsListVM(string fullName) : this()
        {
            SelectedPatient = Patients.FirstOrDefault(p => p.FullName.Equals(fullName));
        }

        private void LoadPatients()
        {
            RestRequest request = new RestRequest($"{Constants.apiAddress}/api/Patients", Method.GET);
            var response = restService.SendRequest(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<List<Patient>>(response.Content);
                Patients = new ObservableCollection<Patient>(data);
            }
        }

        public string SearchText
        {
            get => seartchText;
            set
            {
                seartchText = value;
                OnPropertyChanged();
            }
        }

        public Patient SelectedPatient
        {
            get => selectedPatient;
            set
            {
                selectedPatient = value;
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
            }
        }
        public ObservableCollection<Patient> FilteredPatients
        {
            get => SearchParameter != null ? 
                new ObservableCollection<Patient>(Patients.Where(p => SearchParameter.IsEqual(p, SearchText))) 
                : new ObservableCollection<Patient>(Patients);
        }

        public RelayCommand Select
        {
            get
            {
                return select ?? (select = new RelayCommand(obj =>
                {

                }));
            }
        }
    }
}
