﻿using PharmacyApp.Models.ListViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class AnalizerResearchFinish
    {
        public int? patientId { get; set; }
        public List<ServiceResultModel> services { get; set; }
    }
}