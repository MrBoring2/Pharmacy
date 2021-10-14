using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    class LaboratorianResearcherVM : BasePageVM
    {
        public string Name { get; set; }
        public LaboratorianResearcherVM()
        {
            Name = "Лаборант-исследователь";
        }
    }

}