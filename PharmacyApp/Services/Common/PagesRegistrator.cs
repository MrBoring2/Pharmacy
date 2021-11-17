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
                    RolePages.Add(new LaboratorianMainPageVM());
                    RolePages.Add(new ReceptionOfMaterialVM());
                    break;
                case Roles.LaboratorianResearcher:
                    RolePages.Add(new LaboratorianResearcherMainPageVM());
                    RolePages.Add(new AnalizerWork());
                    break;
                case Roles.Accountant:
                    RolePages.Add(new AccountantVMMainPage());
                    RolePages.Add(new InvoiceIssueVM());
                    RolePages.Add(new ReportPageVM());
                    break;
                case Roles.Administrator:
                    RolePages.Add(new AdministratorVMMainPage());
                    RolePages.Add(new LogginUsersLoginVM());
                    break;
                default:
                    break;
            }
        }
    }
}
