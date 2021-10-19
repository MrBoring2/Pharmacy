using PharmacyApp.Helpers;
using PharmacyApp.ViewModels;
using PharmacyApp.ViewModels.ForPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Services.Common
{
    public class PagesRegistrator
    {
        public List<BasePageVM> RolePages { get; private set; }
        public PagesRegistrator()
        {
            RolePages = new List<BasePageVM>();
        }
        public void RegisterPagesForRole(Roles role)
        {
            switch (role)
            {
                case Roles.Laboratorian:
                    RolePages.Add(new LaboratorianVM());
                    RolePages.Add(new ReceptionOFMaterialVM());
                    break;
                case Roles.LaboratorianResearcher:
                    RolePages.Add(new LaboratorianResearcherVM());
                    break;
                case Roles.Accountant:
                    RolePages.Add(new AccountantVM());
                    break;
                case Roles.Administrator:
                    RolePages.Add(new AdministratorVM());
                    RolePages.Add(new LogginUsersLoginVM());
                    break;
                default:
                    break;
            }
        }
    }
}
