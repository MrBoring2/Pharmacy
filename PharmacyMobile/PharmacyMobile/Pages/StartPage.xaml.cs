using Android;
using PharmacyMobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}