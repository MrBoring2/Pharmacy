using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class BaseModel : ObservableObject
    {
        public object this[string propertyName]
        {
            get
            {
                if(propertyName != string.Empty)
                {
                    var info = this.GetType().GetProperty(propertyName);
                    return info.GetValue(this, null);
                }
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
