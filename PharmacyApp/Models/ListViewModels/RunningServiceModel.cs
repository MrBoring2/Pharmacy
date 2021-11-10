using PharmacyApp.Models.POCOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.ListViewModels
{
    public class NotCompleteService
    {
        public LaboratoryService LaboratoryService { get; set; }
        public int OrderId {get;set;}
        public string Status { get; set; }
        public int Progress { get; set; }
        public double Result { get; set; }
        public bool IsRunning { get; set; }

    }
}
