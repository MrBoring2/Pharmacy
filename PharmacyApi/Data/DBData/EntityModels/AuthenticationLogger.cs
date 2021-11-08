using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApi.Data.DBData.EntityModels
{
    public class AuthenticationLogger
    {
        public int Id { get; set; }
        public DateTime LoginDate { get; set; }
        public bool Attempt { get; set; }
        public virtual User User { get; set; }

    }
}
