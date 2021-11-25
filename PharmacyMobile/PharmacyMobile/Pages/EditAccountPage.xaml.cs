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
    public partial class EditAccountPage : ContentPage
    {
        private string newPassword;


        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(); }
        }
        private string newEmail;

        public string NewEmail
        {
            get { return newEmail; }
            set { newEmail = value; OnPropertyChanged(); }
        }
        private string newPhone;

        public string NewPhone
        {
            get { return newPhone; }
            set { newPhone = value; OnPropertyChanged(); }
        }



        public EditAccountPage()
        {
            InitializeComponent();
            var patient = ClientService.Instance.CurrentPatient;
          
            NewPassword = patient.Password;
            NewEmail = patient.Email;
            NewPhone = patient.Telephone;
            BindingContext = this;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            ClientService.Instance.CurrentPatient.Email = NewEmail;
            ClientService.Instance.CurrentPatient.Password = NewPassword;
            ClientService.Instance.CurrentPatient.Telephone = NewPhone;


            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, 
                ClientService.apiUrl + $"api/patients/{ClientService.Instance.CurrentPatient.PatientId}");

            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(ClientService.Instance.CurrentPatient), Encoding.UTF8, "application/json");
            var response = await ClientService.Instance.PutRequest(httpRequestMessage);
            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Application.Current.Properties["PatientData"] = JsonSerializer.Serialize(ClientService.Instance.CurrentPatient);
                await DisplayAlert("Оповещение", "Профиль успешно обновлён", "ОК");
                await Navigation.PopAsync();
            }
        }

    }
}