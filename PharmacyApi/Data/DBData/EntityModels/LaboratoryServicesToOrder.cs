using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class LaboratoryServicesToOrder
    {
        public int OrderId { get; set; }
        public int LaboratoryServiceId { get; set; }
        public double Result { get; set; }
        public byte[] DateOfFinished { get; set; }
        public bool Accepted { get; set; }
        public string Status { get; set; }
        public int AnalyzerId { get; set; }
        public int UserId { get; set; }

        public virtual Analizer Analyzer { get; set; }
        public virtual LaboratiryService LaboratoryService { get; set; }
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
