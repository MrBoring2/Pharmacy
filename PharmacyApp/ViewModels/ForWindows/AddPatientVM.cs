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
    public class AddPatientVM : BaseWindowVM
    {
        private RestService restService;
        private bool? dialogResult { get; set; }
        private string fullName;
        private DateTime dateOfBirth;
        private string passportNumber;
        private string passportSeries;
        private string telephone;
        private string email;
        private string ein;
        private string sosialSecNumber;
        private string selectedSosialType;
        private string searchCompanyParameter;
        public string SearchCompanyParameter { get => searchCompanyParameter; set { searchCompanyParameter = value; OnPropertyChanged(); OnPropertyChanged(nameof(FilteredCompanies)); } }
        private InsuranceCompany selectedInsuranceCompany;
        private ObservableCollection<InsuranceCompany> insuranceCompanies;
        public ObservableCollection<InsuranceCompany> InsuranceCompanies { get => insuranceCompanies; set { insuranceCompanies = value; OnPropertyChanged(); } }
        public ObservableCollection<InsuranceCompany> FilteredCompanies 
        { 
            get => new ObservableCollection<InsuranceCompany>(InsuranceCompanies
            .Where(p => p.Name.Contains(SearchCompanyParameter) || p.Inn.Contains(SearchCompanyParameter))); 
        }
        public Patient NewPatient {get;set;}
        public AddPatientVM()
        {
            restService = new RestService();
            DateOfBirth = DateTime.Today;
            SocialTypes = new List<string>
            {
                "oms",
                "dms"
            };
            SearchCompanyParameter = string.Empty;
            SelectedSocialType = SocialTypes[0];
            LoadCompanies();
            Add = new RelayCommand(AddNewPatient);
            Cancel = new RelayCommand(CancelWindow);
        }

        private void CancelWindow(object obj)
        {
            DialogResult = false;
        }

        private void LoadCompanies()
        {
            var request = new RestRequest("api/InsuranceСompanies", Method.GET);
            var respons = restService.SendRequest(request);
            if(respons.StatusCode == System.Net.HttpStatusCode.OK)
            {
                InsuranceCompanies = JsonSerializer.Deserialize<ObservableCollection<InsuranceCompany>>(respons.Content);
            }
        }

        private void AddNewPatient(object obj)
        {
            if (Validate())
            {
                CreateLoginAndPassword();
                NewPatient = new Patient
                {
                    FullName = this.FullName,
                    Guid = Guid.NewGuid().ToString(),
                    Login = this.Login,
                    Password = this.Passwrod,
                    DateOfBirth = new DateTimeOffset(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds(),
                    Ein = this.Ein,
                    Email = this.Email,
                    PassportNumber = this.PassportNumber,
                    PassportSeries = this.PassportSeries,
                    Telephone = this.Telephone,
                    SosialSecNumber = this.SosialSecNumber,
                    SosialType = this.SelectedSocialType,
                    InsuranceCompanyId = this.SelectedInsuranceCompany.Id
                };
                DialogResult = true;
            }
            else Notification.ShowNotification("Не все данные заполнены!", "Внимание", NotificationType.Warning);
        }

        private void CreateLoginAndPassword()
        {
            string keys = "abcdefghijklmnopqarstuvwxyzABCDEFGHIJKLMNOPQARSUVWXYZ0123456789";
            Random random = new Random();
            var loginLenght = random.Next(6, 12);
            var passwordLength = random.Next(6, 12);
            for (int i = 0; i < loginLenght; i++)
            {
                Login += keys[random.Next(0, keys.Length)];
            }
            for (int i = 0; i < passwordLength; i++)
            {
                Passwrod += keys[random.Next(0, keys.Length)];
            }
        }

        private bool Validate()
        {
            if (!string.IsNullOrEmpty(FullName)
                && !string.IsNullOrEmpty(PassportNumber)
                && !string.IsNullOrEmpty(Email)
                && !string.IsNullOrEmpty(Telephone)
                && !string.IsNullOrEmpty(SosialSecNumber)
                && !string.IsNullOrEmpty(SelectedSocialType)
                && !string.IsNullOrEmpty(PassportSeries)
                && DateOfBirth >= new DateTime(1900, 1, 1, 0, 0, 0))
                return true;
            else return false;
        }
        public bool? DialogResult { get=>dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
        public string Login { get; set; }
        public string Passwrod { get; set; }
        public List<string> SocialTypes { get; set; }
        public InsuranceCompany SelectedInsuranceCompany { get => selectedInsuranceCompany; set { selectedInsuranceCompany = value;OnPropertyChanged(); } }
        public string FullName { get => fullName; set { fullName = value; OnPropertyChanged(); } }
        public DateTime DateOfBirth { get => dateOfBirth; set { dateOfBirth = value; OnPropertyChanged(); } }
        public string PassportNumber { get => passportNumber; set { passportNumber = value; OnPropertyChanged(); } }
        public string PassportSeries { get => passportSeries; set { passportSeries = value; OnPropertyChanged(); } }
        public string Telephone { get => telephone; set { telephone = value; OnPropertyChanged(); } }
        public string Email { get => email; set { email = value; OnPropertyChanged(); } }
        public string SosialSecNumber { get => sosialSecNumber; set { sosialSecNumber = value; OnPropertyChanged(); } }
        public string SelectedSocialType { get => selectedSosialType; set { selectedSosialType = value; OnPropertyChanged(); } }

        public RelayCommand Add { get; set; }
        public RelayCommand Cancel { get; set; }
        public string Ein { get => ein; set { ein = value; OnPropertyChanged(); } }
    }
}
