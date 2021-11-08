using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PharmacyApp.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для PasswordUserControl.xaml
    /// </summary>
    public partial class PasswordUserControl : UserControl, INotifyPropertyChanged
    {
        bool isPasswordChanging;
        private int maxLength;
        private char passwordChar;
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value);}
        }


        public char PasswordChar { get => passwordChar; set { passwordChar = value; OnPropertyChanged(); } }
        public int MaxLength { get => maxLength; set { maxLength = value; OnPropertyChanged(); } }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is PasswordUserControl passwordUserControl)
            {
                passwordUserControl.UpdatePassword();
            }
        }

        private void UpdatePassword()
        {
            if (!isPasswordChanging)
            {
                PasswordBox.Password = Password;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public PasswordUserControl()
        {
            InitializeComponent();

            // Update DependencyProperty whenever the password changes
            //PasswordBox.PasswordChanged += (sender, args) => {
            //    Password = ((PasswordBox)sender).SecurePassword;
            //};
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            isPasswordChanging = true;
            Password = PasswordBox.Password;
            isPasswordChanging = false;
        }
    }
}

