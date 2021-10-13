using Microsoft.AspNetCore.SignalR.Client;
using PharmacyApp.Helpers;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using RestSharp;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp.ViewModels
{
    public class LiginWindowVM : BaseVM
    {   
        private AuthenticationService authenticationService;
        private System.Timers.Timer timer;
        private string login;
        private string password;
        private string userCapcha;
        private string capcha;
        private bool showPassword;
        private bool capchaCheck;
        private bool isNotBlock;
        private int timerCount;

        public LiginWindowVM()
        {
            Password = string.Empty;
            Login = string.Empty; ;
            UserCapcha = string.Empty; ;
            CapchaCheck = false;
            IsNotBlock = true;
            Capcha = CreateCapcha();
            authenticationService = new AuthenticationService();
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Constants.apiAddress}/pharmacy")
                .Build();
            RestClient restClient = new RestClient(Constants.apiAddress);
            UserService.Instance.InirializeService(hubConnection, restClient);
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
                    Notification notification;
                    if (!CapchaCheck)
                    {
                        var request = authenticationService.Authorization(Login, Password);
                        if (request.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = JsonSerializer.Deserialize<TokenModel>(request.Content);
                            MessageBox.Show($"Добро пожаловать, {data.username}", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            CapchaCheck = true;
                        }
                    }
                    else
                    {
                        var request = authenticationService.Authorization(Login, Password);
                        if (UserCapcha.Equals(Capcha) && request.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = JsonSerializer.Deserialize<TokenModel>(request.Content);
                            MessageBox.Show($"Добро пожаловать, {Login}", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            IsNotBlock = false;
                            timer = new System.Timers.Timer(1000);
                            timer.AutoReset = true;
                            timer.Elapsed += async (sender, e) => await HandleTimer();
                            timer.Start();
                            MessageBox.Show("Неверная капча, логин или пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                }
                ));
            }
        }

        private Task HandleTimer()
        {
            return Task.Run(() =>
            {
               
                timerCount++;

                if (timerCount == 10)
                {
                    timer.Stop();
                    IsNotBlock = true;
                    timerCount = 0;
                }
              
            });
        }

        public RelayCommand ReCreateCapcha
        {
            get
            {
                return reCreateCapcha ?? (reCreateCapcha = new RelayCommand(obj =>
                {
                    Capcha = CreateCapcha();
                }
                ));
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
      
    }
}