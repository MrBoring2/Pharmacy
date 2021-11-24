using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PharmacyApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : INotifyPropertyChanged
    {
        public ObservableCollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
        public ObservableCollection<LaboratoryService> LaboratoryServices { get; set; }
        private RestService restService;
        private DateTime startDate;
        private DateTime endDate;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public ReportPage()
        {
            
            restService = new RestService();
            InitializeComponent();
            ChartServices.ChartAreas.Add(new ChartArea("Main"));
            AvgPeopleAmount.ChartAreas.Add(new ChartArea("Main"));
            ChartResultResearch.ChartAreas.Add(new ChartArea("Main"));

            
            LoadCharts();
            ChartServices.Legends.Add(new Legend("MainLegend"));
            var currentSeries = new Series("Services")
            {
                
                 
            };

            var serviceSeries = new Series("ServicesByTime");
            var resultSeries = new Series("ResultSeries");
            

            ChartServices.Series.Add(currentSeries);
           // ChartServicesByTime.Series.Add(currentSeries);
            ChartResultResearch.Series.Add(currentSeries);
        }

        private void LoadCharts()
        {
            var request = new RestRequest("api/LaboratoryServices", Method.GET);
            var response = restService.SendRequest(request);
            AvgPeopleAmount.Legends.Add("MainLegend");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var services = new ObservableCollection<LaboratoryService>
                    (JsonSerializer.Deserialize<List<LaboratoryService>>(response.Content));
                foreach (var item in services)
                {
                    var countSeries = AvgPeopleAmount.Series.Add($"Service_{item.Code}");
                    countSeries.ChartType = SeriesChartType.Line;
                    var resSeries = ChartResultResearch.Series.Add($"Service_{item.Code}");
                    resSeries.ChartType = SeriesChartType.Line;
                    resSeries.IsVisibleInLegend = true;
                    countSeries.IsVisibleInLegend = true;
                }
            }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var request = new RestRequest("api/LaboratoryServicesToOrders", Method.GET);
            var response = restService.SendRequest(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LaboratoryServicesToOrders = new ObservableCollection<LaboratoryServicesToOrder>
                    (JsonSerializer.Deserialize<List<LaboratoryServicesToOrder>>(response.Content));

                var chartArea = ChartServices.ChartAreas.FirstOrDefault();
               
                Series currentSerise = ChartServices.Series.FirstOrDefault();
                currentSerise.ChartType = SeriesChartType.Line;
                currentSerise.Points.Clear();
                double avgDeviation = 0;
                double avgResult = 0;
                var list = LaboratoryServicesToOrders
                    .Where(p => p.LaboratoryService.Name.Equals(servicesList.SelectedItem as string)).ToList();
                foreach (var item in list)
                {
                    if (item.Result != null)
                    {
                        double.TryParse(item.Result.Replace('.', ','), out double result);
                        if (result != 0)
                        {
                            avgResult += result;
                        }
                    }
                }

                    avgResult /= list.Count;

                for (int i = 0; i < list.Count(); i++)
                {
                    double result = 0;
                    if (list[i] != null)
                    {
                        double.TryParse(LaboratoryServicesToOrders[i].Result.Replace('.', ','), out result);
                        if (result != 0)
                        {
                            avgDeviation += Math.Pow(result - avgResult, 2) / (double)list.Count();
                        }
                    }
                }

                var s1 = avgResult + (avgDeviation * 1); 
                var s2 = avgResult + (avgDeviation * 2); 
                var s3 = avgResult + (avgDeviation * 3);

                var s_1 = avgResult - (avgDeviation * 1);
                var s_2 = avgResult - (avgDeviation * 2);
                var s_3 = avgResult - (avgDeviation * 3);

                var koefVar = (avgDeviation / avgResult) * 100;
                chartArea.AxisX.IsMarginVisible = true;
                chartArea.AxisY.IsStartedFromZero = false;
               
                foreach (var item in list)
                {
                  
                    currentSerise.Points.AddXY(dateTime.AddMilliseconds(Convert.ToDouble(item.DateOfFinished)).ToString(), item.Result); ;
                    
                }
                AvgDevinisionText.Text = Math.Round(avgDeviation, 2).ToString();
                KoefVarText.Text = Math.Round(koefVar, 2).ToString() + "%";
                //currentSerise.Legend = "Легенда";


            }
        }


        private IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        {
            for (var day = start.Date; day.Date <=end.Date;day = day.AddDays(1))
            {
                yield return day;
            }
        }

        private IEnumerable<DateTime> GetDateList(DateTime start, DateTime end)
        {
            var list = new List<DateTime>();
            for (var day = start.Date; day.Date <= end.Date; day = day.AddDays(1))
            {
                list.Add(day);
            }
            return list;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var service = servicesList_Copy.SelectedItem as LaboratoryService;
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            var requestLab = new RestRequest("api/LaboratoryServicesToOrders", Method.GET);
            var responseLab = restService.SendRequest(requestLab);
          
            if (responseLab.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LaboratoryServicesToOrders = new ObservableCollection<LaboratoryServicesToOrder>
                    (JsonSerializer.Deserialize<List<LaboratoryServicesToOrder>>(responseLab.Content));
            }

            var request = new RestRequest("api/LaboratoryServices", Method.GET);
            var response = restService.SendRequest(request);

            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LaboratoryServices = new ObservableCollection<LaboratoryService>
                    (JsonSerializer.Deserialize<List<LaboratoryService>>(response.Content));
                PatientsAmount.Text = LaboratoryServicesToOrders.Count(p =>
                    dateTime.AddMilliseconds(Convert.ToDouble(p.DateOfFinished)) >= StartDate.SelectedDate.Value
                        && dateTime.AddMilliseconds(Convert.ToDouble(p.DateOfFinished)) <= EndDate.SelectedDate.Value).ToString();
                AmountOfServices.Text = LaboratoryServicesToOrders.Count(p =>
                    dateTime.AddMilliseconds(Convert.ToDouble(p.DateOfFinished)) >= StartDate.SelectedDate.Value
                        && dateTime.AddMilliseconds(Convert.ToDouble(p.DateOfFinished)) <= EndDate.SelectedDate.Value).ToString();

                ServicesListText.Text = "";
                foreach (var item in LaboratoryServices)
                {
                    foreach (var item1 in LaboratoryServicesToOrders)
                    {
                        if (dateTime.AddMilliseconds(Convert.ToDouble(item1.DateOfFinished)) >= StartDate.SelectedDate.Value
                        && dateTime.AddMilliseconds(Convert.ToDouble(item1.DateOfFinished)) <= EndDate.SelectedDate.Value
                        && item.Code.Equals(item1.LaboratoryService.Code))
                        {
                            ServicesListText.Text += item.Name + ", ";
                            break;
                        }
                    }
                    
                }

                foreach (var item in LaboratoryServices)
                {
                    
                    var list = new List<LaboratoryServicesToOrder>();
                
                    AvgPeopleAmount.Series.FindByName($"Service_{item.Code}").Points.Clear();
                    foreach (var date in EachDay(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value))
                    {
                        int count = LaboratoryServicesToOrders.Count(p => p.LaboratoryService.Code.Equals(item.Code)
                        && dateTime.AddMilliseconds(Convert.ToDouble(p.DateOfFinished)) >= date
                        && dateTime.AddMilliseconds(Convert.ToDouble(p.DateOfFinished)) <= date.AddDays(1));
                        AvgPeopleAmount.Series.FindByName($"Service_{item.Code}").Points.AddXY(date.Date.ToString(), count);
                    }

                    

                    //foreach (var item1 in LaboratoryServicesToOrders)
                    //{
                    //    if(item.Name.Equals(item1.LaboratoryService.Name))
                    //    {
                    //        count++;
                    //    }
                    //}
                    //resSeries

                    //foreach (var listItem in list)
                    //{
                    //    resultSeries.Points.AddXY(dateTime.AddMilliseconds(Convert.ToDouble(listItem.DateOfFinished)).ToString(), listItem.Result);
                    //}
                   // resultSeries.Points.AddXY(item.Result, );
                }
            }
        }

    }
}
