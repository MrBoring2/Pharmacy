using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnalizerTest
{
    [TestClass]
    public class AnalizerTest
    {
        [TestInitialize]
        public void InitializeFields()
        {
            HttpClient = new HttpClient();
            analizerName = "Ledetect";
            patientId = 2;
            services = new List<string>()
            {
                "229"
            };
        }
        private string analizerName;
        private int patientId;
        private List<string> services;
        private HttpClient HttpClient { get; set; }
        
        [TestMethod]

        public async Task Analizer_SendServiceToReaserch_Success()
        {
            var response = await HttpClient.PostAsync($"http://localhost:63774/api/Analizer/{analizerName}?patientId={patientId}", 
                new StringContent(JsonSerializer.Serialize(services), Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
        [TestMethod]

        public async Task Analizer_RepeatSendServiceToReaserch_Success()
        {
            var response = await HttpClient.PostAsync($"http://localhost:63774/api/Analizer/{analizerName}?patientId={patientId}",
                new StringContent(JsonSerializer.Serialize(services), Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
        [TestMethod]

        public async Task Analizer_GetResearchProgress_Success()
        {
            var response = await HttpClient.GetAsync($"http://localhost:63774/api/Analizer/{analizerName}");
                
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
        //Method AnalizerTest.AnalizerTest.InitializeFields has wrong signature.
        //The method must be non-static, public, does not return a value and should not take any parameter.
        //Additionally, if you are using async-await in method then return-type must be Task.
    }
}
