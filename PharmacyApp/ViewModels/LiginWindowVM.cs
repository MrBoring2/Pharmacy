using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels
{
    public class LiginWindowVM : BaseVM
    {
        private string login;
        private string password;
        private bool showPassword;

        public bool ShowPassword { get => showPassword; set { showPassword = value; OnPropertyChanged(); } }
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        public string Login { get => login; set { login = value; OnPropertyChanged(); } }
    }
}
