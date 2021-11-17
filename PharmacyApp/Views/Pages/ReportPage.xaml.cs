using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public partial class ReportPage
    {
        public ObservableCollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
        public ObservableCollection<LaboratoryService> LaboratoryServices { get; set; }
        private RestService restService;
        public ReportPage()
        {
            
            restService = new RestService();
            InitializeComponent();
            ChartServices.ChartAreas.Add(new ChartArea("Main"));
            ChartServicesByTime.ChartAreas.Add(new ChartArea("Main"));
            ChartResultResearch.ChartAreas.Add(new ChartArea("Main"));


            ChartServices.Legends.Add(new Legend("MainLegend"));
            var currentSeries = new Series("Services")
            {
                
                 
            };

            var serviceSeries = new Series("ServicesByTime");
            var resultSeries = new Series("ResultSeries");

            ChartServices.Series.Add(currentSeries);
            ChartServicesByTime.Series.Add(currentSeries);
            ChartResultResearch.Series.Add(currentSeries);
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

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var service = servicesList_Copy.SelectedItem as LaboratoryService;
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            


            var request = new RestRequest("api/LaboratoryServices", Method.GET);
            var response = restService.SendRequest(request);
            var responseLab = restService.SendRequest(request);
            if (responseLab.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LaboratoryServicesToOrders = new ObservableCollection<LaboratoryServicesToOrder>
                    (JsonSerializer.Deserialize<List<LaboratoryServicesToOrder>>(response.Content));
            }


            var requestLab = new RestRequest("api/LaboratoryServicesToOrders", Method.GET);
           

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LaboratoryServices = new ObservableCollection<LaboratoryService>
                    (JsonSerializer.Deserialize<List<LaboratoryService>>(response.Content));
              
                Series resultSeries = ChartResultResearch.Series.FirstOrDefault();
                foreach (var item in LaboratoryServices)
                {
                    var list = new List<LaboratoryServicesToOrder>();
                    foreach (var item1 in LaboratoryServicesToOrders)
                    {
                        if(item.Name.Equals(item1.LaboratoryService.Name))
                        {
                            list.Add(item1);
                        }
                    }


                    foreach (var listItem in list)
                    {
                       // resultSeries.Points.AddXY(listItem.Analyzer)
                    }
                   // resultSeries.Points.AddXY(item.Result, );
                }
            }
        }

    }
}
