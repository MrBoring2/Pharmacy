using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp.Helpers
{

    public class Notification
    {
        public static void ShowNotification(string message, string title, NotificationType type)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image;

            switch (type)
            {
                case NotificationType.Ok:
                    image = MessageBoxImage.Information;
                    break;
                case NotificationType.Warning:
                    image = MessageBoxImage.Warning;
                    break;
                case NotificationType.Error:
                    image = MessageBoxImage.Error;
                    break;
                default:
                    image = MessageBoxImage.Error;
                    break;
            }

            MessageBox.Show(message, title, button, image);
        }
    }

    public enum NotificationType
    {
        Ok = 1,
        Warning = 2,
        Error = 3
    }
}
