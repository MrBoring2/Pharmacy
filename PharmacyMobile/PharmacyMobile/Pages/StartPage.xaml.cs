using Android;
using PharmacyMobile.Models.POCO_Models;
using PharmacyMobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmacyMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            ClientService.Instance.HttpClient = new System.Net.Http.HttpClient();
        }

        protected override void OnAppearing()
        {
            CheckLogin();
            base.OnAppearing();
        }
        private void CheckLogin()
        {
            bool isLoggenIn = Application.Current.Properties.ContainsKey("IsLoggedIn") ?
            Convert.ToBoolean(Application.Current.Properties["IsLoggedIn"]) : false;

            var patient = Application.Current.Properties.ContainsKey("PatientData") ?
                JsonSerializer.Deserialize<Patient>(Application.Current.Properties["PatientData"].ToString()) : null;

            if (patient != null)
                ClientService.Instance.CurrentPatient = patient;

            ToPersonalAccount.IsVisible = isLoggenIn;
            Login.IsVisible = !isLoggenIn;
            Exit.IsVisible = isLoggenIn;
        }

        private async void News_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsPage() { Title = "Новости лаборатории" });
        }

        private async void Services_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ServicesPage() { Title = "Услуги лаборатории" });
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage() { Title = "Авторизация" });
        }

        private async void ToPersonalAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonalAccountPage() { Title = "Личный кабинет" });
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["IsLoggedIn"] = bool.FalseString;
            CheckLogin();
        }

    }
}