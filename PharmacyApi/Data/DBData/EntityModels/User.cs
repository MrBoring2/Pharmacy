﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public DateTime LastEnterDate { get; set; }
        public string ServicesCodes { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
