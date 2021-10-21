using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class InsuranceCompany : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Inn { get; set; }
        public string CheckingAccount { get; set; }
        public string Bic { get; set; }
        public string Country { get; set; }
    }
}
