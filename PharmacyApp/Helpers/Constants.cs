using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Helpers
{
    public class Constants
    {
        public const string apiAddress = "http://localhost:50764";
    }

    public enum Roles
    {
        Laboratorian = 1,
        LaboratorianResearcher = 2,
        Accountant = 3,
        Administrator = 4
    }
}
