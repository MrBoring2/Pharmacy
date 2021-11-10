using PharmacyApp.Helpers;
using PharmacyApp.Models.ListViewModels;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels.ForWindows;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class AnalizerWork : BasePageVM
    {
        private RestService restService;
        public AnalizerWork()
        {
            PageName = "Работа с анализатором";
            restService = new RestService();
            LoadAnalizers();
            Select = new RelayCommand(SelectAnalizer);  
        }

        private void SelectAnalizer(object obj)
        {
            WindowNavigation.Instance.OpenWindow(new AnalizerVM(SelectedAnalizer));
        }

        private void LoadAnalizers()
        {
            Analizers = new List<AnalizerView>();
            var request = new RestRequest("api/Analizers", Method.GET);
            var response = restService.SendRequest(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                var data = JsonSerializer.Deserialize<List<Analizer>>(response.Content);
                foreach (var analizer in data)
                {
                    var newAnalizer = new AnalizerView();
                    newAnalizer.AnalizerName = analizer.AnalizerName;
                    newAnalizer.IsBuzy = false;
                    foreach (var service in analizer.LaboratoryServiceToAnalizer)
                    {
                        newAnalizer.Services.Add(service);
                    }
                    Analizers.Add(newAnalizer);
                }
            }
        }
       

        public List<AnalizerView> Analizers { get; set; }
        private AnalizerView seelctedAnalizer;
        public AnalizerView SelectedAnalizer
        {
            get => seelctedAnalizer;
            set
            {
                seelctedAnalizer = value;
                OnPropertyChanged();
            }
        } 

        public RelayCommand Select { get; set; } 
    }
}
