using Microsoft.Toolkit.Mvvm.Input;
using PharmacyApp.Helpers;
using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.ViewModels.ForWindows;
using PharmacyApp.Views.Windows;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace PharmacyApp.ViewModels.ForPages
{
    public class InvoiceIssueVM : BasePageVM
    {
        private RestService restService;
        private InsuranceCompany insuranceCompany;
        public InsuranceCompany InsuranceCompany { get => insuranceCompany; set { insuranceCompany = value; OnPropertyChanged(); } }

        private DateTime startDate;
        public DateTime StartDate { get => startDate; set { startDate = value; OnPropertyChanged(); } }
        private DateTime endDate;
        public DateTime EndDate { get => endDate; set { endDate = value; OnPropertyChanged(); } }
        public RelayCommand SelectCompany { get; set; }
        public RelayCommand CreateIssue { get; set; }
        public InvoiceIssueVM()
        {
            PageName = "Сформировать счёт";
            restService = new RestService();
            SelectCompany = new RelayCommand(SelectCompanyAsync);
            CreateIssue = new RelayCommand(CreateNewIuuse);
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        private async void CreateNewIuuse()
        {
            var b = new DateTimeOffset(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
            var request = new RestRequest("api/Orders/getByDate", Method.GET)
                .AddParameter("insuranceCompanyId", InsuranceCompany.Id)
                .AddParameter("dateStart", new DateTimeOffset(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds())
                .AddParameter("dateEnd", new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds());
            var orders = new List<Order>();
            var response = restService.SendRequest(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                orders = JsonSerializer.Deserialize<List<Order>>(response.Content);
                decimal totalPrice = 0;

                await Task.Run(() =>
                {
                    var app = new Word.Application();
                    Word.Document document = app.Documents.Add();

                    Word.Paragraph title = document.Paragraphs.Add();
                    title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    title.Range.Font.Size = 25;
                    title.Range.Text = $"Счёт на команию {InsuranceCompany.Name}";
                    title.Range.InsertParagraphAfter();
                    foreach (var order in orders)
                    {
                        var paragraph = document.Paragraphs.Add();
                        paragraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        paragraph.Range.Font.Size = 20;
                        paragraph.Range.Text = $"Пациент: {order.Patient.FullName}";
                        paragraph.Range.InsertParagraphAfter();

                        var tableParagrath = document.Paragraphs.Add();
                        tableParagrath.Range.Font.Size = 16;
                        tableParagrath.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        tableParagrath.Range.Text = $"Услуги: ";
                        Word.Table table = document.Tables.Add(tableParagrath.Range, order.LaboratoryServicesToOrders.Count() + 1, 3);
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
                        tableParagrath.Range.InsertParagraphAfter();

                        var priceParagraph = document.Paragraphs.Add();
                        priceParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        priceParagraph.Range.Font.Size = 18;
                        priceParagraph.Range.Text = $"Полная стоиомть услуг пациента: {order.LaboratoryServicesToOrders.Sum(p => p.LaboratoryService.Price)} руб.";
                        priceParagraph.Range.InsertParagraphAfter();
                        totalPrice += order.LaboratoryServicesToOrders.Sum(p => p.LaboratoryService.Price);

                    }
                    var totalPriceParagraph = document.Paragraphs.Add();
                    totalPriceParagraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    totalPriceParagraph.Range.Font.Size = 25;
                    totalPriceParagraph.Range.Text = $"Полная стоиомть: {totalPrice} руб.";

                    document.SaveAs2(AppDomain.CurrentDomain.BaseDirectory + $@"../../Счета страховых компаний/Счёт на команию №{Guid.NewGuid()}_ИНН-{InsuranceCompany.Inn}_Название-{InsuranceCompany.Name}.pdf", Word.WdExportFormat.wdExportFormatPDF);
                });

                var postRequest = new RestRequest("api/Orders", Method.POST)
                    .AddJsonBody(new InvoiceIssue
                    {
                        InsuranceCompanyId = InsuranceCompany.Id,
                        Price = totalPrice,
                        StartPeriod = new DateTimeOffset(StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, StartDate.Minute, StartDate.Second, TimeSpan.Zero).ToUnixTimeSeconds(),
                        EndPeriod = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, EndDate.Hour, EndDate.Minute, EndDate.Second, TimeSpan.Zero).ToUnixTimeSeconds(),
                        UserId = UserService.Instance.UserId
                    });


                Notification.ShowNotification("Счёт успешно сформирован, электроннй вариант создан в папке Счета страховых компаний.", "Оповещение", NotificationType.Ok);
            }
        }

        private async void SelectCompanyAsync()
        {
            var companiesVM = new InsuranceCompaniesVM();
            await Task.Run(() => { WindowNavigation.Instance.OpenModalWindow(companiesVM); });
            if (companiesVM.DialogResult == true)
            {
                InsuranceCompany = companiesVM.SelectedCompany;
            }
        }
    }
}
