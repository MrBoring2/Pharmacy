using PharmacyApp.Helpers;
using PharmacyApp.ViewModels;
using PharmacyApp.ViewModels.ForWindows;
using PharmacyApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            WindowNavigation.Instance.RegisterWindow<LoginWindowVM, LoginWindow>();
            WindowNavigation.Instance.RegisterWindow<MainAppWindowVM, MainAppWindow>();
            WindowNavigation.Instance.RegisterWindow<PatientsListVM, PatientsList>();
            WindowNavigation.Instance.RegisterWindow<AddPatientVM, AddPatient>();
            WindowNavigation.Instance.RegisterWindow<ServicesListVM, ServicesList>();
            WindowNavigation.Instance.RegisterWindow<AnalizerVM, Analizer>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            WindowNavigation.Instance.OpenWindow(new LoginWindowVM());
        }
    }
}
