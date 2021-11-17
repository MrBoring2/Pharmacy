using PharmacyApp.Models.POCOModels;
using PharmacyApp.Services.ApiServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class ReportPageVM : BasePageVM
    {
        private RestService restService;
        public List<string> ServicesNames { get; set; }
        public ReportPageVM()
        {
            PageName = "Отчёты";
            restService = new RestService();
            LoadServices();
        }

        private void LoadServices()
        {
            var request = new RestRequest("api/LaboratoryServices", Method.GET);
            var response = restService.SendRequest(request);
            ServicesNames = new List<string>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonSerializer.Deserialize<List<LaboratoryService>>(response.Content);
                foreach (var item in list)
                {
                    ServicesNames.Add(item.Name);
                }
            }
        }
    }
}
