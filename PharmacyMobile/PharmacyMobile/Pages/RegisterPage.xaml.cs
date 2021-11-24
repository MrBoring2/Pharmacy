using PharmacyMobile.Models.POCO_Models;
using PharmacyMobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmacyMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private string fio;

        public string FIO
        {
            get { return fio; }
            set { fio = value; OnPropertyChanged(); }
        }
        private string login;

        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        private string repeatPassword;

        public string RepeatPassword
        {
            get { return repeatPassword; }
            set { repeatPassword = value; OnPropertyChanged(); }
        }

        private string sequriyNumber;

        public string SequrityNumber
        {
            get { return sequriyNumber; }
            set { sequriyNumber = value; OnPropertyChanged(); }
        }

        private DateTime dateOfBirth;

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(); }
        }

        private string passportNumber;

        public string PassportNumber
        {
            get { return passportNumber; }
            set { passportNumber = value; OnPropertyChanged(); }
        }
        private string passportSeries;

        public string PassportSeries
        {
            get { return passportSeries; }
            set { passportSeries = value; OnPropertyChanged(); }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void register_Clicked(object sender, EventArgs e)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ClientService.apiUrl}api/Patients");
            httpRequestMessage.Content = new System.Net.Http.StringContent(JsonSerializer.Serialize(
                new Patient
                {
                    Ein = "",
                    Login = Login,
                    Password = Password,
                    DateOfBirth = new DateTimeOffset(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    Email = Email,
                    FullName = FIO,
                    Guid = Guid.NewGuid().ToString(),
                    InsuranceCompanyId = null,
                    IpAddress = "",
                    PassportNumber = PassportNumber,
                    PassportSeries = PassportSeries,
                    SosialSecNumber = SequrityNumber,
                    SosialType = "",
                    Telephone = Phone,
                    Ua = "",
                }), Encoding.UTF8, "application/json");

            var request = await ClientService.Instance.PostRequest(httpRequestMessage);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Вы услешно зарегистрировались!", "Оповещение", "ОК");
                await Navigation.PushAsync(new LoginPage());
            }
        }
    }
}