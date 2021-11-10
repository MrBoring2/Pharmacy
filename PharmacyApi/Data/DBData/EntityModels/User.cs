using PharmacyApi.Data.DBData.EntityModels;
using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class User
    {
        public User()
        {
            AuthenticationLoggers = new HashSet<AuthenticationLogger>();
            LaboratoryServicesToOrders = new HashSet<LaboratoryServicesToOrder>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public DateTime LastEnterDate { get; set; }
        public string ServicesCodes { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<AuthenticationLogger> AuthenticationLoggers { get; set; }
        public virtual ICollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
    }
}
