using Aspose.BarCode.Generation;
using NetBarcode;
using PharmacyApp.Helpers;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels.ForWindows;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class ReceptionOfMaterialVM : BasePageVM
    {
        private RestService restSerivce;
        private string barcodeInput;
        private string patientFullName;
        private string serviceName;
        private byte[] barcodeImage { get; set; }
        private RelayCommand createBarcode;
        private RelayCommand readBarcode;
        private RelayCommand findService;
        private RelayCommand addService;
        private RelayCommand removeService;
        private LaboratoryService selectedService;
        private ObservableCollection<LaboratoryService> services;
        public ObservableCollection<LaboratoryService> Services { get => services; set { services = value; OnPropertyChanged(); } }
        public LaboratoryService SelectedService { get => selectedService; set { selectedService = value; OnPropertyChanged(); } } 

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

        public RelayCommand FindPatient { get; set; }
        public RelayCommand FindService
        {
            get
            {
                return findService ?? (findService = new RelayCommand(obj =>
                {
                    if (!string.IsNullOrEmpty(ServiceName))
                    {
                        var request = new RestRequest($"api/LaboratoryServices/getByName/{ServiceName}", Method.GET);
                        var response = restSerivce.SendRequest(request);

                        if(response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if(!IsExistService(JsonSerializer.Deserialize<LaboratoryService>(response.Content)))
                                Services.Add(JsonSerializer.Deserialize<LaboratoryService>(response.Content));
                            else Notification.ShowNotification("Сервис уже находится в списке!", "Внимание", NotificationType.Warning);
                        }
                        else Notification.ShowNotification("Услуга не найдена!", "Внимание", NotificationType.Error);
                    }
                    else Notification.ShowNotification("Строка пустая!", "Внимание", NotificationType.Error);
                }));
            }
        }

        private bool IsExistService(LaboratoryService service)
        {
            foreach (var item in Services)
            {
                if (item.Name.Equals(service.Name))
                    return true;
            }
            return false;
        }

        public RelayCommand AddService { get; set; }
        public RelayCommand RemoveService
        {
            get
            {
                return removeService ?? (removeService = new RelayCommand(obj =>
                {
                    if (SelectedService != null)
                    {
                        Services.Remove(SelectedService);
                    }
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
            restSerivce = new RestService();
            PageName = "Принять биоматериал";
            Services = new ObservableCollection<LaboratoryService>();
            byte[] bytes = CreateBarcodeFromString("0000000000000").GetByteArray();
            BarcodeImage = bytes;
            FindPatient = new RelayCommand(FindPatientAsync);
            AddService = new RelayCommand(AddServiceAsync);
        }

        private async void AddServiceAsync(object obj)
        {
            var servicesVM = new ServicesListVM();
            await Task.Run(() => WindowNavigation.Instance.OpenModalWindow(servicesVM));
            if (servicesVM.DialogResult == true)
            {
                if (!IsExistService(servicesVM.SelectedService))
                    Services.Add(servicesVM.SelectedService);
                else Notification.ShowNotification("Сервис уже находится в списке!", "Внимание", NotificationType.Warning);
            }
        }

        private async void FindPatientAsync(object obj)
        {
            var patientsVM = new PatientsListVM();
            await Task.Run(() => WindowNavigation.Instance.OpenModalWindow(patientsVM));
            if(patientsVM.DialogResult == true)
            {
                PatientFullName = patientsVM.SelectedPatient.FullName;
            }
            
        }

        private bool ValidateBarcodeInput(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                foreach (var ch in barcode)
                {
                    if (!char.IsDigit(ch))
                        return false;
                }

                return true;
            }
            return false;
        }


        private Barcode CreateBarcodeFromString(string barcode)
        {
            try
            {
                var bar = new Barcode(barcode, NetBarcode.Type.EAN13, true, 230, 100, LabelPosition.BottomCenter, AlignmentPosition.Center, Color.White, Color.Black, new Font("Courier New", 12));
                return bar;
            }
            catch(Exception ex)
            {
                Notification.ShowNotification("Коротий штрих код!", "Внимание", NotificationType.Error);
            }
            return CreateBarcodeFromString("0000000000000");
        }
    }
}
