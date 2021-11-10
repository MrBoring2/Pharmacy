using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class LaboratoryService
    {
        public LaboratoryService()
        {
            LaboratoryServiceToAnalizers = new HashSet<LaboratoryServiceToAnalizer>();
            LaboratoryServicesToOrders = new HashSet<LaboratoryServicesToOrder>();
        }

        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public virtual ICollection<LaboratoryServiceToAnalizer> LaboratoryServiceToAnalizers { get; set; }
        [JsonIgnore]
        public virtual ICollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
    }
}
