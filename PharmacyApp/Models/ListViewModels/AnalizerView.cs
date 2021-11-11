using PharmacyApp.Models.POCOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.ListViewModels
{
    public class AnalizerView
    {
        public AnalizerView()
        {
            Services = new List<LaboratoryServiceToAnalizer>();
        }
        public int AnalizerId { get; set; }
        public string AnalizerName { get; set; }
        public bool IsBuzy { get; set; }
        public List<LaboratoryServiceToAnalizer> Services { get; set; }
    }
}
