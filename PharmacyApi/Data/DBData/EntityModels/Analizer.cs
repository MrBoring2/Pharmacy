using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class Analizer
    {
        public Analizer()
        {
            LaboratoryServicesToOrders = new HashSet<LaboratoryServicesToOrder>();
        }

        public int Id { get; set; }
        public string AnalizerName { get; set; }

        public virtual ICollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
    }
}
