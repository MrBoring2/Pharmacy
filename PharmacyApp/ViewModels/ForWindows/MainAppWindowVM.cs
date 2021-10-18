using Microsoft.Toolkit.Mvvm.Input;
using PharmacyApp.Helpers;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels.ForPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class MainAppWindowVM : BaseWindowVM
    {

        private Dispatcher dispatcher;
        private Timer timer;
        private TimeSpan Time = new TimeSpan();
        private BasePageVM _currentPageVM;
        private List<BasePageVM> _pageVMs;
        private PagesRegistrator PagesRegistrator { get; set; }

        public MainAppWindowVM(Roles role)
        {
            SetTimer();
            PagesRegistrator = new PagesRegistrator();
            PagesRegistrator.RegisterPagesForRole(role);
            PageVMs.AddRange(PagesRegistrator.RolePages);
            dispatcher = Dispatcher.CurrentDispatcher;
            CurentPageVM = PageVMs[0];
        }
        private Services.Common.RelayCommand _changePageCommand;

        public Services.Common.RelayCommand ChangePageCommand
        {
            get
            {
                return _changePageCommand ??
                    ( _changePageCommand = new Services.Common.RelayCommand(obj =>
                {
                    ChangePageVM((BasePageVM)obj);
                }));
            }
        }

        public string DisplayTime
        {
            get => $"Время (ч:м) - {Time.Hours}:{Time.Minutes}";
        }

        public string Test
        {
            get => $"{Time.Seconds}";
        }

        public List<BasePageVM> PageVMs
        {
            get
            {
                if(_pageVMs == null)
                    _pageVMs = new List<BasePageVM>();
                return _pageVMs;
            }
        }

        public BasePageVM CurentPageVM
        {
            get => _currentPageVM;
            set
            {
                if (_currentPageVM != value)
                {
                    _currentPageVM = value;
                    OnPropertyChanged("CurentPageVM");
                }
            }
        }


        private void SetTimer()
        {
            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Start();
        }

        private Task HandleTimer()
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


        private void ReturnToLogin()
        {
            UserService.Instance.HubConnection.StopAsync();
            WindowNavigation.Instance.OpenAndHideWindow(this, new LoginWindowVM(true));
        }

        private void ChangePageVM(BasePageVM pageVM)
        {
            if (!PageVMs.Contains(pageVM))
                PageVMs.Add(pageVM);

            CurentPageVM = PageVMs
                .FirstOrDefault(vm => vm == pageVM);
        }
    }
}
