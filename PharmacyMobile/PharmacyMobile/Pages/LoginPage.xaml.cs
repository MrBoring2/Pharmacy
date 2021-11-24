using PharmacyMobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmacyMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private string login;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }



        public LoginPage()
        {
            InitializeComponent();

            TapGestureRecognizer registerRecognizer = new TapGestureRecognizer();
            registerRecognizer.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new RegisterPage() { Title = "Региcтрация" });
            };

            registerLink.GestureRecognizers.Add(registerRecognizer);
            BindingContext = this;
        }

        private async void enter_Clicked(object sender, EventArgs e)
        {
            var response = await ClientService.Instance.PostRequest("api/authPatient",
                new StringContent(JsonSerializer.Serialize(new { Login = Login, Password = Password })));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Вы вошли!", "Оповещение", "ОК");
            }
        }
    }
}