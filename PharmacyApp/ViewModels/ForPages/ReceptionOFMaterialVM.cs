using Aspose.BarCode.Generation;
using NetBarcode;
using PharmacyApp.Helpers;
using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels.ForWindows;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class ReceptionOfMaterialVM : BasePageVM
    {
        private string barcodeInput;
        private string patientFullName;
        private string serviceName;
        private byte[] barcodeImage { get; set; }
        private RelayCommand createBarcode;
        private RelayCommand readBarcode;
        private RelayCommand findPatient;
        private RelayCommand findService;
        private RelayCommand addService;
        private RelayCommand removeService;
      
        public string BarcodeInput
        {
            get => barcodeInput;
            set
            {
                barcodeInput = value;
                OnPropertyChanged();
            }
        }
        public string PatientFullName
        {
            get => patientFullName;
            set
            {
                patientFullName = value;
                OnPropertyChanged();
            }
        }

        public string ServiceName
        {
            get => serviceName;
            set
            {
                serviceName = value;
                OnPropertyChanged();
            }
        }

        public byte[] BarcodeImage { get=>barcodeImage; set { barcodeImage = value; OnPropertyChanged(); }}

        public RelayCommand CreateBarcode
        {
            get
            {
                return createBarcode ?? (createBarcode = new RelayCommand(obj =>
                {
                    if (ValidateBarcodeInput(BarcodeInput))
                        BarcodeImage = CreateBarcodeFromString(BarcodeInput).GetByteArray();
                }));
            }
        }
        public RelayCommand ReadBarcode
        {
            get
            {
                return readBarcode ?? (readBarcode = new RelayCommand(obj =>
                {
                    BarcodeInput = Read();
                    BarcodeImage = CreateBarcodeFromString(BarcodeInput).GetByteArray();
                }));
            }
        }

        public RelayCommand FindPatient
        {
            get
            {
                return findPatient ?? (findPatient = new RelayCommand(obj =>
                {                   
                    WindowNavigation.Instance.OpenModalWindow(new PatientsListVM());
                }));
            }
        }
        public RelayCommand FindService
        {
            get
            {
                return findService ?? (findService = new RelayCommand(obj =>
                {

                }));
            }
        }

        public RelayCommand AddService
        {
            get
            {
                return addService ?? (addService = new RelayCommand(obj =>
                {

                }));
            }
        }
        public RelayCommand RemoveService
        {
            get
            {
                return removeService ?? (removeService = new RelayCommand(obj =>
                {

                }));
            }
        }

        private string Read()
        {
            string barcode = string.Empty;
            Random random = new Random();
            foreach (var item in Enumerable.Range(1, 13))
            {
                barcode += random.Next(0, 10);
            }
            return barcode;
        }

        public ReceptionOfMaterialVM()
        {
            PageName = "Принять биоматериал";
            
            byte[] bytes = CreateBarcodeFromString("0000000000000").GetByteArray();
            BarcodeImage = bytes;
        }

        private bool ValidateBarcodeInput(string barcode)
        {
            foreach (var ch in barcode)
            {
                if (!char.IsDigit(ch))
                    return false;              
            }
            return true;
        }


        private Barcode CreateBarcodeFromString(string barcode)
        {
            return new Barcode(barcode, NetBarcode.Type.EAN13, true, 230, 100, LabelPosition.BottomCenter, AlignmentPosition.Center, Color.White, Color.Black, new Font("Courier New", 12));
        }
    }
}
