using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PharmacyApp.Helpers.Converters
{
    public class ConvertTimeStamp : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            int b = 0;
            if(Int32.TryParse(value.ToString(), out b))
                return dateTime.AddSeconds(System.Convert.ToDouble(value));
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
