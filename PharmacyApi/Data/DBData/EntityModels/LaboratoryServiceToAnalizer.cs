using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class LaboratoryServiceToAnalizer
    {
        public int AnalizerId { get; set; }
        public int LaboratoryServiceId { get; set; }

        public virtual Analizer Analizer { get; set; }
        public virtual LaboratoryService LaboratoryService { get; set; }
    }
}
