using PharmacyApp.Helpers;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class LogginUsersLoginVM : BasePageVM
    {
        private RestService restSevice;

        public LogginUsersLoginVM()
        {
            PageName = "История входа";
            restSevice = new RestService();
            LoadLogs();
        }

      
        
        private ObservableCollection<AuthenticationLogger> logs;

        public ObservableCollection<AuthenticationLogger> Logs
        {
            get => logs;
            set
            {
                logs = value;
                OnPropertyChanged();
            }
        }

        private void LoadLogs()
        {
            var request = new RestRequest(Constants.apiAddress + "/api/AuthenticationLoggers", Method.GET);
            var response = restSevice.SendRequest(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<List<AuthenticationLogger>>(response.Content);
                Logs = new ObservableCollection<AuthenticationLogger>(data);
            }
        }
    }
}
