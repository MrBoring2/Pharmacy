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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Word = Microsoft.Office.Interop.Word;

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
        private Patient CurrentPatient { get; set; }
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

        public byte[] BarcodeImage { get => barcodeImage; set { barcodeImage = value; OnPropertyChanged(); } }

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

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (!IsExistService(JsonSerializer.Deserialize<LaboratoryService>(response.Content)))
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

        public RelayCommand CreateOrder { get; set; }

        public ReceptionOfMaterialVM()
        {
            restSerivce = new RestService();
            PageName = "Принять биоматериал";
            Services = new ObservableCollection<LaboratoryService>();
            byte[] bytes = CreateBarcodeFromString("0000000000000").GetByteArray();
            BarcodeImage = bytes;
            FindPatient = new RelayCommand(FindPatientAsync);
            AddService = new RelayCommand(AddServiceAsync);
            CreateOrder = new RelayCommand(CreateNewOrder);
        }

        private async void CreateNewOrder(object obj)
        {
            if (Validate())
            {
                decimal totalPrice = 0;
                foreach (var item in Services)
                {
                    totalPrice += item.Price;
                }

                var dateRequest = new RestRequest("api/helper/time", Method.GET);
                var date = JsonSerializer.Deserialize<DateTime>(restSerivce.SendRequest(dateRequest).Content);

                Order order = new Order
                {
                    DateOfCreation = new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, TimeSpan.Zero).ToUnixTimeSeconds().ToString(),
                    Barcode = BarcodeInput,
                    PatientId = CurrentPatient.PatientId
                };

                List<LaboratoryServicesToOrder> ServicesInOrder = new List<LaboratoryServicesToOrder>();
                foreach (var item in Services)
                {
                    ServicesInOrder.Add(
                        new LaboratoryServicesToOrder
                        {
                            LaboratoryService = item,
                            Result = null,
                            DateOfFinished = null,
                            Accepted = null,
                            Status = "Not completed",
                            AnalyzerId = null,
                            UserId = null
                        });
                }
                order.LaboratoryServicesToOrders = ServicesInOrder;

                var createOrderRequest = new RestRequest("api/Orders", Method.POST).AddJsonBody(order);
                var response = restSerivce.SendRequest(createOrderRequest);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var createdOrder = JsonSerializer.Deserialize<Order>(response.Content);
                    await Task.Run(() => CreatePdfDocument(CurrentPatient, createdOrder));
                    Notification.ShowNotification("Заказа успешно оформлен, документ создан в папке Электронные документы", "Оповещение", NotificationType.Ok);

                    BarcodeInput = string.Empty;
                    PatientFullName = string.Empty;
                    Services = new ObservableCollection<LaboratoryService>();
                    BarcodeImage = CreateBarcodeFromString("0000000000000").GetByteArray();
                    var temp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    var orderDate = temp.AddSeconds(Convert.ToDouble(order.DateOfCreation));
                    string line = $"https://wsrussia.ru/?data=base64(дата_заказа={orderDate.Year}-{orderDate.Month}-{orderDate.Day}T{orderDate.Hour}:{orderDate.Minute}:{orderDate.Second}&номер_заказа={createdOrder.Id})\n";
                    File.AppendAllText("../../Ссылки о заказах/link.txt", line);
                }
            }
            else Notification.ShowNotification("Не все поля заполнены!", "Внимание", NotificationType.Ok);
        }

        private bool Validate()
        {
            var standartBarcode = CreateBarcodeFromString("0000000000000").GetByteArray(); ;
            if (BarcodeImage != standartBarcode
                && !string.IsNullOrEmpty(PatientFullName)
                && Services.Count > 0)
                return true;
            return false;
                  

        }

        private void CreatePdfDocument(Patient patient, Order order)
        {
            var app = new Word.Application();
            Word.Document document = app.Documents.Add();


            Word.Paragraph title = document.Paragraphs.Add();
            title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.Font.Size = 25;
            title.Range.Text = $"Заказ №{order.Id}";
            title.Range.InsertParagraphAfter();

            Word.Paragraph paragraph1 = document.Paragraphs.Add();
            paragraph1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph1.Range.Font.Size = 16;
            var temp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            paragraph1.Range.Text = $"Дата заказа: {temp.AddMilliseconds(Convert.ToDouble(order.DateOfCreation))}";
            paragraph1.Range.InsertParagraphAfter();

            Word.Paragraph paragraph2 = document.Paragraphs.Add();
            paragraph2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph2.Range.Font.Size = 16;
            paragraph2.Range.Text = $"Номер пробирки: {order.Barcode}";
            paragraph2.Range.InsertAfter("\nШтрих-код: ");
            paragraph2.Range.InsertParagraphAfter();
            using (var ms = new MemoryStream(BarcodeImage))
            {
                var img = Image.FromStream(ms);
                img.Save(@"barcode.png", ImageFormat.Png);
                Word.InlineShape barcodeShape = paragraph2.Range.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "barcode.png");
                barcodeShape.Title = "Штрих-код";
                File.Delete(@"barcode.png");
            }
            paragraph2.Range.InsertParagraphAfter();

            Word.Paragraph paragraph3 = document.Paragraphs.Add();
            paragraph3.Range.Font.Size = 16;
            paragraph3.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph3.Range.Text = $"Номер страхового полиса: {CurrentPatient.SosialSecNumber}";
            paragraph3.Range.InsertParagraphAfter();

            Word.Paragraph paragraph4 = document.Paragraphs.Add();
            paragraph4.Range.Font.Size = 16;
            paragraph4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph4.Range.Text = $"ФИО: {CurrentPatient.FullName}";
            paragraph4.Range.InsertParagraphAfter();

            Word.Paragraph paragraph5 = document.Paragraphs.Add();
            paragraph5.Range.Font.Size = 16;
            paragraph5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph5.Range.Text = $"Дата рождения: {temp.AddMilliseconds(CurrentPatient.DateOfBirth)}";
            paragraph5.Range.InsertParagraphAfter();

            Word.Paragraph paragraph6 = document.Paragraphs.Add();
            paragraph6.Range.Font.Size = 16;
            paragraph6.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph6.Range.Text = $"Услуги: ";
            Word.Table table = document.Tables.Add(paragraph6.Range, Services.Count() + 1, 3);
            table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;
            cellRange = table.Cell(1, 1).Range;
            cellRange.Text = "Код";
            cellRange = table.Cell(1, 2).Range;
            cellRange.Text = "Название";
            cellRange = table.Cell(1, 3).Range;
            cellRange.Text = "Стоимость";

            table.Rows[1].Range.Bold = 1;
            table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            int index = 1;
            foreach (var item in order.LaboratoryServicesToOrders)
            {
                cellRange = table.Cell(index + 1, 1).Range;
                cellRange.Text = item.LaboratoryService.Code.ToString();

                cellRange = table.Cell(index + 1, 2).Range;
                cellRange.Text = item.LaboratoryService.Name.ToString();

                cellRange = table.Cell(index + 1, 3).Range;
                cellRange.Text = item.LaboratoryService.Price.ToString() + " руб.";
                index++;
            }
            paragraph6.Range.InsertParagraphAfter();

            Word.Paragraph priceParagraph = document.Paragraphs.Add();
            priceParagraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            priceParagraph.Range.Font.Size = 25;
            priceParagraph.Range.Text = $"Полная стоиомть: {order.LaboratoryServicesToOrders.Sum(p => p.LaboratoryService.Price)} руб.";

            document.SaveAs2(AppDomain.CurrentDomain.BaseDirectory + $@"../../Электронные документы/Заказ №{order.Id}.pdf", Word.WdExportFormat.wdExportFormatPDF);

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
            if (patientsVM.DialogResult == true)
            {
                CurrentPatient = patientsVM.SelectedPatient;
                PatientFullName = CurrentPatient.FullName;
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
                var bar = new Barcode(barcode, NetBarcode.Type.EAN13, true, 230, 100, LabelPosition.BottomCenter, AlignmentPosition.Center, System.Drawing.Color.White, System.Drawing.Color.Black, new Font("Courier New", 12));
                return bar;
            }
            catch (Exception ex)
            {
                Notification.ShowNotification("Коротий штрих код!", "Внимание", NotificationType.Error);
            }
            return CreateBarcodeFromString("0000000000000");
        }
    }
}
