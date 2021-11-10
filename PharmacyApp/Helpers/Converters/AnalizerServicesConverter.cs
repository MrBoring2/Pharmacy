using PharmacyApp.Models.POCOModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PharmacyApp.Helpers.Converters
{
    public class AnalizerServicesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string services = string.Empty;
            int index = 0;
            foreach (var item in value as List<LaboratoryServiceToAnalizer>)
            {
                services += item.LaboratoryService.Name;
                if (index != (value as List<LaboratoryServiceToAnalizer>).Count - 1)
                    services += ", ";
                index++;
            }
            return services;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
