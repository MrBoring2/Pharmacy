using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class AnalizerWork
    {
        public int AnalizerId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDateOfReceipt { get; set; }
        public DateTime OrderDateOfcompletion { get; set; }

        public virtual Analizer Analizer { get; set; }
        public virtual Order Order { get; set; }
    }
}
