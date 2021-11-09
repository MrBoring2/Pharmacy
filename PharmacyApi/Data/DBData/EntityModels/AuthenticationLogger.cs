using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class AuthenticationLogger
    {
        public int Id { get; set; }
        public DateTime LoginDate { get; set; }
        public bool Attempt { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
