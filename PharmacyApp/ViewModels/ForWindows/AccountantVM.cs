using PharmacyApp.Helpers;
using PharmacyApp.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class AccountantVM : MainAppWindowVM
    {
        public AccountantVM()
        {
            PagesRegistrator = new PagesRegistrator();
            PagesRegistrator.RegisterPagesForRole(Roles.Accountant);
            PageVMs.AddRange(PagesRegistrator.RolePages);
            CurentPageVM = PageVMs[0];
        }
    }
}
