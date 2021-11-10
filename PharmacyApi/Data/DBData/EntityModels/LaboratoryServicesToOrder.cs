using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class LaboratoryServicesToOrder
    {
        public int OrderId { get; set; }
        public int LaboratoryServiceId { get; set; }
        public double? Result { get; set; }
        public string DateOfFinished { get; set; }
        public bool? Accepted { get; set; }
        public string Status { get; set; }
        public int? AnalyzerId { get; set; }
        public int? UserId { get; set; }

        public virtual Analizer Analyzer { get; set; }
        public virtual LaboratoryService LaboratoryService { get; set; }
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
