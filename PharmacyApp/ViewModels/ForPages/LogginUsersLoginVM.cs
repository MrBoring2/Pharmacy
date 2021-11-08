using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using PharmacyApp.Helpers;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class LogginUsersLoginVM : BasePageVM
    {
        private RestService restSevice;
        private DateTime dateStart;
        private DateTime dateEnd;
        public LogginUsersLoginVM()
        {
            PageName = "История входа";
            restSevice = new RestService();
            LoadLogs();
            DateEnd = DateTime.Now;
            DateStart = Logs.Min(p => p.LoginDate);

            UserService.Instance.HubConnection.On<string>("UpdateLogs", (logs) =>
            {
                Logs = new ObservableCollection<AuthenticationLogger>(JsonConvert.DeserializeObject<List<AuthenticationLogger>>(logs));
                OnPropertyChanged(nameof(FilterLogs));
            });
        }

        public DateTime DateStart
        {
            get => dateStart;
            set
            {
                if (dateStart != DateTime.MinValue)
                {
                    if (value.Date <= DateEnd.Date)
                    {
                        dateStart = value;
                        OnPropertyChanged();
                        OnPropertyChanged("FilterLogs");
                    }
                    else Notification.ShowNotification("Дата начала не может быть больше даты конца!", "Внимание", NotificationType.Warning);
                }
                else dateStart = value;
            }
        }
        public DateTime DateEnd
        {
            get => dateEnd;
            set
            {
                if (dateEnd != DateTime.MinValue)
                {
                    if (value.Date >= DateStart.Date)
                    {
                        dateEnd = value;
                        OnPropertyChanged();
                        OnPropertyChanged("FilterLogs");
                    }
                    else Notification.ShowNotification("Дата конца не может быть меньше даты начала!", "Внимание", NotificationType.Warning);
                }
                else dateEnd = value;
            }
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

        public ObservableCollection<AuthenticationLogger> FilterLogs
        {
            get => new ObservableCollection<AuthenticationLogger>(Logs.Where(p => p.LoginDate.Date >= DateStart.Date && p.LoginDate.Date <= DateEnd.Date).OrderBy(p=>p.LoginDate));
        }

        private void LoadLogs()
        {
            var request = new RestRequest(Constants.apiAddress + "/api/AuthenticationLoggers", Method.GET);
            var response = restSevice.SendRequest(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<List<AuthenticationLogger>>(response.Content);
                Logs = new ObservableCollection<AuthenticationLogger>(data);
            }
        }

 
    }
}
