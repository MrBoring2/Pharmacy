using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class Order
    {
        public Order()
        {
            LaboratoryServicesToOrders = new HashSet<LaboratoryServicesToOrder>();
        }

        public int Id { get; set; }
        public int PatientId { get; set; }
        public string DateOfCreation { get; set; }
        public string Barcode { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual ICollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
    }
}
