using Android;
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
        }

        private async void News_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsPage());
        }

        private void Services_Clicked(object sender, EventArgs e)
        {

        }
    }
}