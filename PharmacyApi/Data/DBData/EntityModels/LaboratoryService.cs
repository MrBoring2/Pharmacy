using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class LaboratoryService
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
