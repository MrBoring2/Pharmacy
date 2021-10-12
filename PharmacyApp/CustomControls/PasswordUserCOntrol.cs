using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PharmacyApp.CustomControls
{
    public class PasswordUserControl : UserControl
    {
        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
        }
        //public static readonly DependencyProperty PasswordProperty = 
        //    DependencyProperty.Register("Password", typeof(SecureString)
        //        , typeof)
    }
}
