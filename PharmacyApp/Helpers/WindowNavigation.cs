using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp.Helpers
{
    public class WindowNavigation
    {
        private static WindowNavigation insance;
        private DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        private static object syncRoot = new Object();

        public static WindowNavigation Instance
        {
            get
            {
                if (insance == null)
                {
                    lock (syncRoot)
                    {
                        if (insance is null)
                            insance = new WindowNavigation();
                    }
                }             
                return insance;
            }
        }

        public void RegisterWindow<VM, Win>()
            where VM : class
            where Win : Window
        {
            displayRootRegistry.RegisterWindowType<VM, Win>();
        }

        public void OpenWindow(BaseWindowVM newWindowVM)
        {
            
            if (newWindowVM != null)
            {
                displayRootRegistry.ShowPresentation(newWindowVM);
            }
        }

        public void OpenAndHideWindow(BaseWindowVM currentWindowVM, BaseWindowVM newWindowVM)
        {
            if(currentWindowVM != null && newWindowVM != null)
            {
                displayRootRegistry.ShowPresentation(newWindowVM);
                displayRootRegistry.HidePresentation(currentWindowVM);
            }
        }

        public void OpenModalWindow(BaseWindowVM windowVM)
        {
            if (windowVM != null)
            {
               displayRootRegistry.ShowModalPresentation(windowVM);
            }
        }


    }
}
