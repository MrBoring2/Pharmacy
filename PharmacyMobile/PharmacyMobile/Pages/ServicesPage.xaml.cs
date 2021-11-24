using PharmacyMobile.Models.POCO_classes;
using PharmacyMobile.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmacyMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesPage : ContentPage
    {
        public ObservableCollection<LaboratoryService> Services { get; set; }

        public ServicesPage()
        {
            InitializeComponent();
            LoadServices();

            BindingContext = this;

        }

        private async void LoadServices()
        {
            var response = await ClientService.Instance.GetRequest("api/LaboratoryServices");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Services = JsonSerializer.Deserialize<ObservableCollection<LaboratoryService>>(response.Content.ReadAsStringAsync().Result);
            }
            ServicesListView.ItemsSource = Services;
        }

        private void ServicesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
