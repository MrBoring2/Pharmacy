using PharmacyApp.Helpers;
using PharmacyApp.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class LaboratorianResearcherVM : LaboratorianVM
    {
        public LaboratorianResearcherVM()
        {
            PagesRegistrator = new PagesRegistrator();
            PagesRegistrator.RegisterPagesForRole(Roles.LaboratorianResearcher);
            PageVMs.AddRange(PagesRegistrator.RolePages);
            CurentPageVM = PageVMs[0];
        }
    }
}
