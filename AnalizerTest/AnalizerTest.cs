using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnalizerTest
{
    /// <summary>
    ///  ласс тестировани€ анализатора, цифра в названии метода - пор€док выполнени€
    /// </summary>
    [TestClass]
    public class AnalizerTest
    {
        private readonly string analizerUrl = "http://localhost:63774";
        private HttpClient HttpClient { get; set; }

        /// <summary>
        /// »нициализаци€ данныз дл€ тестов, анализаторов 2 - Ledetect и Biorad
        /// ƒоступные услуги дл€ Leditrct - "229", "258", "311", "323", "346", "415", "501", "543", "557", "619", "659"
        /// ƒоступные услуги дл€ Biorad - "176", "258", "287", "543", "548", "619", "659", "797", "836", "855"
        /// Id пациента мождет быть любым
        /// </summary>
        [TestInitialize]
        public void InitializeFields()
        {
            HttpClient = new HttpClient();         
        }

        /// <summary>
        /// ќтправить услуги на исследование, когда анализатор не зан€т, должен вернутьс€ статус '200 (OK)'
        /// </summary>
        /// <returns>200 (OK)</returns>
        [TestMethod]
        public async Task _3_SendServices_WhenAlanizerNotBusy_ShoudReturnOk()
        {
            //Arrange
            string analizerName = "Ledetect";
            int patientId = 2;
            List<string> services = new()
            {
                "229", "258"
            };

            //Act
            var response = await HttpClient.PostAsync($"{analizerUrl}/api/Analizer/{analizerName}?patientId={patientId}",
                    new StringContent(JsonSerializer.Serialize(services), Encoding.UTF8, "application/json"));

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// ќтправить услуги на исследование, когда анализатор зан€т, должен вернутьс€ статус '400 (BadRequest)' и
        /// вывести 'Analizer is busy' 
        /// </summary>
        /// <returns>400 (BadRequest)</returns>
        [TestMethod]
        public async Task _4_SendServices_WhenAlanizerIsBusy_ShouldReturnBadRequest()
        {
            //Arrange
            string analizerName = "Ledetect";
            int patientId = 2;
            List<string> services = new()
            {
                "229", "258"
            };
                
            //Act
            var response = await HttpClient.PostAsync($"{analizerUrl}/api/Analizer/{analizerName}?patientId={patientId}",
                new StringContent(JsonSerializer.Serialize(services), Encoding.UTF8, "application/json"));

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// ѕолучить прогресс исследовани€, когда услуги наход€тс€ в процессе исследовани€, должен вернутьс€ статус '200 (ќ )'
        /// </summary>
        /// <returns>200 (ќ )</returns>
        [TestMethod]
        public async Task _5_GetResearchProgress_WhenServisesOnResearch_ShouldReturnOk()
        {
            //Arrange
            string analizerName = "Ledetect";

            //Act
            var response = await HttpClient.GetAsync($"{analizerUrl}/api/Analizer/{analizerName}");

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// ѕолучить прогресс исследовани€, когда услуги не наход€тс€ в процессе исследовани€, должен вернутьс€ статус '400 (BadRequest)' и
        /// вывести 'Analyzer is not working'
        /// </summary>
        /// <returns>400 (BadRequest)</returns>
        [TestMethod]
        public async Task _1_GetResearchProgress_WhenServisesNotOnResearch_ShouldReturnBadRequest()
        {
            //Arrange
            string analizerName = "Ledetect";

            //Act
            var response = await HttpClient.GetAsync($"{analizerUrl}/api/Analizer/{analizerName}");

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// ќтправить услуги на исследование, когда в запросе есть услуги, недоступные дл€ указанного анализатора, должне вернутьс€ статус '400 (BadReqeust)' и
        /// вывести 'Analyzer can not do this order.May be order contains services which analyzer does not support'
        /// </summary>
        /// <returns>400 (BadRequest)</returns>
        [TestMethod]
        public async Task _2_SendServices_WhichNotAvailableForAnalizer_ShoudReturnBadReqeust()
        {
            //Arrange
            string analizerName = "Ledetect";
            int patientId = 2;
            List<string> notAvailableServices = new()
            {
                "111", "234"
            };

            //Act
            var response = await HttpClient.PostAsync($"{analizerUrl}/api/Analizer/{analizerName}?patientId={patientId}",
                new StringContent(JsonSerializer.Serialize(notAvailableServices), Encoding.UTF8, "application/json"));

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// ќтправить услуги на исследование, когда в запросе указан не существующий анализатор, должен вернутс€ статус '400 (BadRequest)' и
        /// вывести Analizer with name '{name}' not found
        /// </summary>
        /// <returns>400 (BadRequest)</returns>
        [TestMethod]
        public async Task _6_SendServices_ToNotExistingAnalizer_ShoudReturnBadReqeust()
        {
            //Arrange
            string notExistingAnalzierName = "Alamba";
            int patientId = 2;
            List<string> services = new()
            {
                "229", "258"
            };

            //Act
            var response = await HttpClient.PostAsync($"{analizerUrl}/api/Analizer/{notExistingAnalzierName}?patientId={patientId}",
                new StringContent(JsonSerializer.Serialize(services), Encoding.UTF8, "application/json"));

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// ѕолучить результат услуги, когда в запросе указан не существующий анализатор, должен вернутс€ статус '400 (BadRequest)' и
        /// вывести Analizer with name '{name}' not found
        /// </summary>
        /// <returns>400 (BadRequest)</returns>
        [TestMethod]
        public async Task _7_GetResearchProgress_FromNotExistingAnalizer_ShoudReturnBadReqeust()
        {
            //Arrange
            string notExistingAnalzierName = "Grande";

            //Act
            var response = await HttpClient.GetAsync($"{analizerUrl}/api/Analizer/{notExistingAnalzierName}");

            //Assert
            Trace.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
    }
}
