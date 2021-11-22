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
using System.Windows;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class PatientsListVM : BaseWindowVM
    {
        private bool? dialogResult;
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
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
            SearchText = string.Empty;
            AddPatient = new RelayCommand(AddPatientAsync);
            Edit = new RelayCommand(EditPatientAsync);
        }

        private async void EditPatientAsync(object obj)
        {
            if (SelectedPatient != null)
            {
                var editPatientVM = new EditPatientVM(SelectedPatient);
                await Task.Run(() => WindowNavigation.Instance.OpenModalWindow(editPatientVM));

                if (editPatientVM.DialogResult == true)
                {
                    var request = new RestRequest($"api/Patients/{editPatientVM.CurrentPatient.PatientId}", Method.PUT);
                    request.AddJsonBody(JsonSerializer.Serialize(editPatientVM.CurrentPatient));
                    var response = restService.SendRequest(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        LoadPatients();
                        OnPropertyChanged(nameof(FilteredPatients));
                    }
                }
            }
        }

        private async void AddPatientAsync(object obj)
        {
            var addPatientVM = new AddPatientVM();
            await Task.Run(() => WindowNavigation.Instance.OpenModalWindow(addPatientVM));

            if (addPatientVM.DialogResult == true)
            {
                var request = new RestRequest("api/patients", Method.POST).AddJsonBody(JsonSerializer.Serialize(addPatientVM.NewPatient));
                var response = restService.SendRequest(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    LoadPatients();
                    OnPropertyChanged(nameof(FilteredPatients));
                }
            }
        }

        public PatientsListVM(string fullName) : this()
        {
            SelectedPatient = Patients.FirstOrDefault(p => p.FullName.Equals(fullName));
        }

        private void LoadPatients()
        {
            RestRequest request = new RestRequest($"{Constants.apiAddress}/api/Patients", Method.GET);
            var response = restService.SendRequest(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
                OnPropertyChanged(nameof(FilteredPatients));
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
                SearchText = string.Empty;
                OnPropertyChanged(nameof(FilteredPatients));
            }
        }
        public ObservableCollection<Patient> FilteredPatients
        {
            get => SearchParameter != null && SearchParameter.Property != string.Empty ?
                new ObservableCollection<Patient>(Patients.Where(p => SearchParameter.IsEqual(p, SearchText)))
                : new ObservableCollection<Patient>(Patients);
        }
        public RelayCommand AddPatient { get; set; }
        public RelayCommand Edit { get; set; }
        public RelayCommand Select
        {
            get
            {
                return select ?? (select = new RelayCommand(obj =>
                {
                    if (SelectedPatient != null)
                    {
                        DialogResult = true;
                    }
                    else Notification.ShowNotification("Пациент не выбран!", "Внимание", NotificationType.Warning);
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
