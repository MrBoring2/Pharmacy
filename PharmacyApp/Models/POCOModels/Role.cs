using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class Role : BaseModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
