using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels
{

    //Базовы класс для всех ViewModel
    public class BasePageVM : ObservableObject
    {
        public string PageName { get; set; }
    }
}
