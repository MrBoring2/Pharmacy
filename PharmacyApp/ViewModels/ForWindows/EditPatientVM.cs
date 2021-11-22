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
    public class EditPatientVM : BaseWindowVM
    {
        public Patient CurrentPatient { get; set; }
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
        public EditPatientVM(Patient patient)
        {
            CurrentPatient = patient;
            restService = new RestService();
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateOfBirth = dateTime.AddMilliseconds(CurrentPatient.DateOfBirth);
            FullName = CurrentPatient.FullName;
            Ein = CurrentPatient.Ein;
            Email = CurrentPatient.Email;
            PassportNumber = CurrentPatient.PassportNumber;
            PassportSeries = CurrentPatient.PassportSeries;
            Telephone = CurrentPatient.Telephone;
            SosialSecNumber = CurrentPatient.SosialSecNumber;
            SelectedSocialType = CurrentPatient.SosialType;
           

            SocialTypes = new List<string>
            {
                "oms",
                "dms"
            };
            SearchCompanyParameter = string.Empty;
            SelectedSocialType = SocialTypes[0];
            LoadCompanies();

            SelectedInsuranceCompany = InsuranceCompanies.FirstOrDefault(p => p.Id == CurrentPatient.InsuranceCompany.Id);
            Add = new RelayCommand(EditPatient);
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
            if (respons.StatusCode == System.Net.HttpStatusCode.OK)
            {
                InsuranceCompanies = JsonSerializer.Deserialize<ObservableCollection<InsuranceCompany>>(respons.Content);
            }
        }

        private void EditPatient(object obj)
        {
            if (Validate())
            {
                CurrentPatient.FullName = FullName;
                CurrentPatient.Ein = Ein;
                CurrentPatient.Email = Email;
                CurrentPatient.InsuranceCompanyId = SelectedInsuranceCompany.Id;
                CurrentPatient.PassportNumber = PassportNumber;
                CurrentPatient.PassportSeries = PassportSeries;
                CurrentPatient.SosialSecNumber = SosialSecNumber;
                CurrentPatient.Telephone = Telephone;
                CurrentPatient.SosialType = SelectedSocialType;
                CurrentPatient.DateOfBirth = new DateTimeOffset(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

                DialogResult = true;
            }
            else Notification.ShowNotification("Не все данные заполнены!", "Внимание", NotificationType.Warning);
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
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
        public string Login { get; set; }
        public string Passwrod { get; set; }
        public List<string> SocialTypes { get; set; }
        public InsuranceCompany SelectedInsuranceCompany { get => selectedInsuranceCompany; set { selectedInsuranceCompany = value; OnPropertyChanged(); } }
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
