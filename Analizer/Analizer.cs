using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer
{
    public class Analizer
    {
        public string Name { get; set; }
        public List<string> AvailableLaboratoryServicesCode { get; set; }
        public bool IsBusy { get; set; }
        public int Progress { get; set; }
    }
}
