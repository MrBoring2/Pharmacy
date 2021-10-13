using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Services.Common
{
    public class NavigationService
    {
        private static volatile NavigationService instance;
        private static object syncRoot = new Object();

        private NavigationService() { }

        private static NavigationService Instance
        {
            get
            {
                lock(syncRoot)
                {
                    if (instance == null)
                        instance = new NavigationService();
                }
                return instance;
            }
        }
    }
}
