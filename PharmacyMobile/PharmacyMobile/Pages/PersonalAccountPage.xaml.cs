using PharmacyMobile.Models.POCO_Models;
using PharmacyMobile.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmacyMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalAccountPage : ContentPage
    {
        private string fio;
        private string login;
        private string password;
        private string sequriyNumber;
        private DateTime dateOfBirth;
        private string passportNumber;
        private string passportSeries;
        private string phone;
        private string email;

        public PersonalAccountPage()
        {
            InitializeComponent();
            InitializeFields();
            BindingContext = this;
        }

        private void InitializeFields()
        {
            var patient = ClientService.Instance.CurrentPatient;
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            FIO = patient.FullName;
            Login = patient.Login;
            Password = patient.Password;
            SequrityNumber = patient.SosialSecNumber;
            DateOfBirth = dateTime.AddMilliseconds(patient.DateOfBirth);
            PassportNumber = patient.PassportNumber;
            PassportSeries = patient.PassportSeries;
            Phone = patient.Telephone;
            Email = patient.Email;
        }

        public string FIO
        {
            get { return fio; }
            set { fio = value; OnPropertyChanged(); }
        }
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        public string SequrityNumber
        {
            get { return sequriyNumber; }
            set { sequriyNumber = value; OnPropertyChanged(); }
        }
        public string PassportNumber
        {
            get { return passportNumber; }
            set { passportNumber = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(); }
        }
        public int Age => DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? DateTime.Now.Year - DateOfBirth.Year + 1 :
            DateTime.Now.Year - DateOfBirth.Year;
        public string PassportSeries
        {
            get { return passportSeries; }
            set { passportSeries = value; OnPropertyChanged(); }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(); }
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditAccountPage() { Title = "Рдеактирование профиля" });
        }
        private void UpdateAccount()
        {
            Password = ClientService.Instance.CurrentPatient.Password;
            Phone = ClientService.Instance.CurrentPatient.Telephone;
            Email = ClientService.Instance.CurrentPatient.Email;
        }

        protected override void OnAppearing()
        {
            UpdateAccount();
            base.OnAppearing();
        }
    }
}