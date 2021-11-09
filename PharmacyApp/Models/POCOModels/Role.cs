﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class Role : BaseModel
    {
        [JsonPropertyName("id")]

        public int Id { get; set; }
        [JsonPropertyName("roleName")]

        public string RoleName { get; set; }
    }
}
