using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.ListViewModels
{
    public class AnalizerPostModel
    {
        public string patientId { get; set; }
        public List<ServiceModel> services { get; set; }

    }
}
