using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class LaboratorianVM : BasePageVM
    {
        public string Name { get; set; }
        public LaboratorianVM()
        {
            Name = "Лаборант";
        }
    }
}
