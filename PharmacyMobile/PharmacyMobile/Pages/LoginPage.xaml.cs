using PharmacyMobile.Models.POCO_Models;
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
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, ClientService.apiUrl + "api/authPatient");
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(new { Login = Login, Password = Password }), Encoding.UTF8, "application/json");
            var response = await ClientService.Instance.PostRequest(httpRequestMessage);
            if(response == null)
            {
                await DisplayAlert("Внимание", "Не удалось подлкючиться к серверу!", "ОК");
                return;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Application.Current.Properties["IsLoggedIn"] = bool.TrueString;
                ClientService.Instance.CurrentPatient = JsonSerializer.Deserialize<Patient>(response.Content.ReadAsStringAsync().Result);
                Application.Current.Properties["PatientData"] = JsonSerializer.Serialize(ClientService.Instance.CurrentPatient);
                await DisplayAlert("Оповещение", "Вы вошли!", "ОК");
                await Navigation.PopAsync();
            }
        }
      
    }
}