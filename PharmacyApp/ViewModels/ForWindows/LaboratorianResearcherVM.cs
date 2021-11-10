using PharmacyApp.Helpers;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class LaboratorianResearcherVM : MainAppWindowVM
    {
        protected Timer timer;
        protected TimeSpan Time = new TimeSpan();
        protected Dispatcher dispatcher;
        public LaboratorianResearcherVM()
        {
            PagesRegistrator = new PagesRegistrator();
            PagesRegistrator.RegisterPagesForRole(Roles.LaboratorianResearcher);
            PageVMs.AddRange(PagesRegistrator.RolePages);
            dispatcher = Dispatcher.CurrentDispatcher;
            CurentPageVM = PageVMs[0];
            SetTimer();
        }
        protected void SetTimer()
        {
            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Start();
        }
        public string DisplayTime
        {
            get => $"Время (ч:м) - {Time.Hours}:{Time.Minutes}";
        }

        protected Task HandleTimer()
        {
            return Task.Run(() =>
            {
                Time = Time.Add(new TimeSpan(0, 0, 1));

                if (Constants.seans_time.Subtract(Time) == TimeSpan.FromMinutes(Constants.seans_end_notification))
                {
                    Notification.ShowNotification($"Внимание, время сеанса походит к концу, выход из приложения произойдёт через {Constants.seans_time}!",
                        "Внимание", NotificationType.Ok);
                }

                if (Constants.seans_time.Subtract(Time) > TimeSpan.Zero)
                {
                    OnPropertyChanged("DisplayTime");
                }
                else
                {
                    timer.Stop();
                    OnPropertyChanged("DisplayTime");
                    Notification.ShowNotification("Время сейнса вышло, вкоре будет проверено кварцевание помещения!", "Внимание", NotificationType.Ok);
                    dispatcher.Invoke(ReturnToLogin);
                }
            });
        }


        protected void ReturnToLogin()
        {
            UserService.Instance.HubConnection.StopAsync();
            WindowNavigation.Instance.OpenAndHideWindow(this, new LoginWindowVM(true));
        }
    }
}
