using PharmacyApp.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class AnalizerResultVM : BaseWindowVM
    {
        private string result;
        public string Result { get => result; set { result = value; OnPropertyChanged(); } }
        private bool? dialogResult;
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
        public AnalizerResultVM(string result)
        {
            Result = result;
            Accept = new RelayCommand(AcceptResult);
            Reject = new RelayCommand(RejectResult);
        }

        public RelayCommand Accept { get; set; }
        public RelayCommand Reject { get; set; }


        private void RejectResult(object obj)
        {
            DialogResult = false;
        }

        private void AcceptResult(object obj)
        {
            DialogResult = true;
        }

    }
}
