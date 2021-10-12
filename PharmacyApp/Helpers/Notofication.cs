using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp.Helpers
{

    public class Notofication
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType NotificationType { get; set; }

        public Notofication(string title, string content, NotificationType notificationType)
        {
            Title = title;
            Content = content;
            NotificationType = notificationType;
        }

        public void ShowNotification()
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image;

            switch (NotificationType)
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

            MessageBox.Show(Content, Title, button, image);
        }
    }

    public enum NotificationType
    {
        Ok = 1,
        Warning = 2,
        Error = 3
    }
}
