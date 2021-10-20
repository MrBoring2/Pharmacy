using Microsoft.AspNetCore.SignalR.Client;
using PharmacyApp.Helpers;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using PharmacyApp.Views.Windows;
using RestSharp;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class LoginWindowVM : BaseWindowVM
    {   
        private RestService restService;
        private System.Timers.Timer timer;
        private string login;
        private string password;
        private string userCapcha;
        private string capcha;
        private bool showPassword;
        private bool capchaCheck;
        private bool isNotBlock;
        private TimeSpan time;
        private Dispatcher dispatcher;

        public LoginWindowVM()
        {
            Initiazlize();
        }

        public LoginWindowVM(bool block) : this()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            IsNotBlock = false;
            timer.AutoReset = true;
            timer.Elapsed += async (sender, e) => await Quartzization();
            timer.Start();
            
            dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(()=> Notification.ShowNotification($"Блокировка на {Constants.quartzization_time_minutes} минут, происходит кварцевание помещения!",
                "Оповещение", NotificationType.Ok)));
        }

        private Task Quartzization()
        {
            return Task.Run(() =>
            {
                time.Add(new TimeSpan(0, 0, 1));

                if (time.Minutes == Constants.quartzization_time_minutes)
                {
                    IsNotBlock = true;
                    timer.Stop();
                    Notification.ShowNotification("Кварцевание помещения закончено!", "Оповещение", NotificationType.Ok);
                }
            });
        }

        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        public string Login { get => login; set { login = value; OnPropertyChanged(); } }
        public string Capcha { get => capcha; set { capcha = value; OnPropertyChanged(); } }
        public string UserCapcha { get => userCapcha; set { userCapcha = value; OnPropertyChanged(); } }
        public bool ShowPassword { get => showPassword; set { showPassword = value; OnPropertyChanged(); } }
        public bool CapchaCheck { get => capchaCheck; set { capchaCheck = value; OnPropertyChanged(); } }
        public bool IsNotBlock { get => isNotBlock; set { isNotBlock = value; OnPropertyChanged(); } }

        private RelayCommand reCreateCapcha;
        private RelayCommand authorize;


        public RelayCommand Authorize
        {
            get
            {
                return authorize ?? (authorize = new RelayCommand(obj =>
                {

                    try
                    {
                        if (!CapchaCheck)
                        {
                            UserService.Instance.HubConnection.StartAsync();
                            var request = restService.Authorization(Login, Password);
                            if (request.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var data = JsonSerializer.Deserialize<TokenModel>(request.Content);
                                LoginUser(data);
                            }
                            else
                            {
                                Notification.ShowNotification("Неверный логин или пароль", "Внимание", NotificationType.Warning);
                                CapchaCheck = true;
                            }
                        }
                        else
                        {
                            var request = restService.Authorization(Login, Password);
                            if (UserCapcha.Equals(Capcha) && request.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var data = JsonSerializer.Deserialize<TokenModel>(request.Content);
                                LoginUser(data);
                            }
                            else
                            {
                                IsNotBlock = false;
                                Capcha = CreateCapcha();
                                timer.AutoReset = true;
                                timer.Elapsed += async (sender, e) => await HandleTimer();
                                timer.Start();
                                Notification.ShowNotification("Неверная капча, логин или пароль", "Внимание", NotificationType.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }));
            }
        }

        private Task HandleTimer()
        {
            return Task.Run(() =>
            {

                time = time.Add(new TimeSpan(0, 0, 1));

                if (time.Seconds == 10)
                {
                    timer.Stop();
                    IsNotBlock = true;
                    time = TimeSpan.Zero;
                }
              
            });
        }
        private void Initiazlize()
        {
            Password = string.Empty;
            Login = string.Empty; ;
            UserCapcha = string.Empty; ;
            CapchaCheck = false;
            IsNotBlock = true;
            timer = new System.Timers.Timer(1000);
            Capcha = CreateCapcha();
            restService = new RestService();
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Constants.apiAddress}/pharmacy",
                options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(UserService.Instance.Token);
                }).Build();
            RestClient restClient = new RestClient(Constants.apiAddress);
            UserService.Instance.InirializeService(hubConnection, restClient);
            SetSignalREvents();
        }

        private void LoginUser(TokenModel data)
        {
            UserService.Instance.SetUser(data.user_name_surname, data.user_login, data.role_name,
                                    (Roles)Convert.ToInt32(data.role_id), data.access_token);

            UserService.Instance.HubConnection.StartAsync();

            WindowNavigation.Instance.OpenAndHideWindow(this, SelectWindow((Roles)Convert.ToInt32(data.role_id)));          
        }

        public RelayCommand ReCreateCapcha
        {
            get
            {
                return reCreateCapcha ?? (reCreateCapcha = new RelayCommand(obj =>
                {
                    Capcha = CreateCapcha();
                }));
            }
        }

        private MainAppWindowVM SelectWindow(Roles role)
        {
            switch (role)
            {
                case Roles.Laboratorian:
                    return new LaboratorianVM();
                case Roles.LaboratorianResearcher:
                    return new LaboratorianResearcherVM();
                case Roles.Accountant:
                    return new AccountantVM();
                case Roles.Administrator:
                    return new AdministratorVM();
                default:
                    throw new ArgumentException("Роль не найдена!");
            }
        }

        private string CreateCapcha()
        {
            string codes = "ABCDEFGHIGKLMNOPQARSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string capcha = string.Empty;
            Random r = new Random();
            for (int i = 0; i < r.Next(4, 12); i++)
            {
                capcha += codes[r.Next(0, codes.Length)];
            }

            return capcha;
        }

        private void SetSignalREvents()
        {
            UserService.Instance.HubConnection.On<string>("Welcome", (message) => Notification.ShowNotification(message, "Оповещение", NotificationType.Ok));
        }      


    }
}