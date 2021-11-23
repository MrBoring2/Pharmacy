using Android.Content.Res;
using PharmacyMobile.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmacyMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public ObservableCollection<NewsModel> NewsItems { get; set; }

        public NewsPage()
        {
            InitializeComponent();
            LoadNews();
            BindingContext = this;
        }

        private async void LoadNews()
        {
            var sd = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PharmacyMobile.Resources.Data_News.json");
            string text = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                text = await reader.ReadToEndAsync();
            }

            NewsItems = JsonSerializer.Deserialize<ObservableCollection<NewsModel>>(text);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
           

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
