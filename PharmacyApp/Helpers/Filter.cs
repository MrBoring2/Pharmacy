using PharmacyApp.Models.POCOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Helpers
{
    public class Filter
    {
        public Filter(string title, string property)
        {
            Title = title;
            Property = property;
        }

        public string Title { get; set; }
        public string Property { get; set; }

        public bool IsEqual(object obj, string search)
        {
            if (obj is Patient p)
            {
                var value = p[Property];
                return value.ToString().ToLower().Contains(search.ToLower());
            }
            return false;
}
    }
}
