using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class LaboratoryService
    {
        public LaboratoryService()
        {
            LaboratoryServicesToOrders = new HashSet<LaboratoryServicesToOrder>();
        }

        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
    }
}
