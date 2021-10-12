using PharmacyApp.Services.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp.ViewModels
{
    public class LiginWindowVM : BaseVM
    {
        public LiginWindowVM()
        {
            Password = string.Empty;
            Login = string.Empty; ;
            UserCapcha = string.Empty; ;
            CapchaCheck = false;
            IsNotBlock = true;
            Capcha = CreateCapcha();
        }
        System.Timers.Timer timer;
        private string login;
        private string password;
        private string userCapcha;
        private string capcha;
        private bool showPassword;
        private bool capchaCheck;
        private bool isNotBlock;
        private int loginTryCount;
        private int timerCount;

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
                    if (!CapchaCheck)
                        if (Password == "123")
                            MessageBox.Show("Вы вошли");
                        else
                        {
                            MessageBox.Show("Ошибка входа");
                            CapchaCheck = true;
                        }
                    else
                    {
                        if (UserCapcha.Equals(Capcha) && Password.Equals("123"))
                            MessageBox.Show("Вы вошли");
                        else
                        {
                            IsNotBlock = false;
                            // TimerCallback tm = new TimerCallback(TimerStepCount);
                            timer = new System.Timers.Timer(1000);
                            timer.AutoReset = true;
                            timer.Elapsed += async (sender, e) => await HandleTimer();
                            timer.Start();
                            MessageBox.Show("Ждите 10 секунд");
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
