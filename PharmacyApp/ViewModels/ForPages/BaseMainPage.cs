using PharmacyApp.Services.ApiServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class BaseMainPage : BasePageVM
    { 
        public string Name { get; set; } = UserService.Instance.UserNameSurname.Split(' ')[0];
        public string Surname { get; set; } = UserService.Instance.UserNameSurname.Split(' ')[1];
        public string RoleName { get; set; } = UserService.Instance.RoleName;

        public BaseMainPage()
        {
            PageName = "Главная страница";
        }
    }
}
