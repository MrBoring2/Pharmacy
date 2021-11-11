using PharmacyApp.Models.POCOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.ListViewModels
{
    public class NotCompleteService : BaseModel
    {
        public LaboratoryService LaboratoryService { get; set; }
        public Order Order {get;set;}
        public string Status { get; set; }
        public int Progress { get; set; }
        public string Result { get; set; }
        public bool IsRunning { get; set; }

    }
}
