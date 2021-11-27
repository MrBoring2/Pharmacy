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
    /// ����� ������������ �����������, ����� � �������� ������ - ������� ����������
    /// </summary>
    [TestClass]
    public class AnalizerTest
    {
        private readonly string analizerUrl = "http://localhost:63774";
        private HttpClient HttpClient { get; set; }

        /// <summary>
        /// ������������� ������ ��� ������, ������������ 2 - Ledetect � Biorad
        /// ��������� ������ ��� Leditrct - "229", "258", "311", "323", "346", "415", "501", "543", "557", "619", "659"
        /// ��������� ������ ��� Biorad - "176", "258", "287", "543", "548", "619", "659", "797", "836", "855"
        /// Id �������� ������ ���� �����
        /// </summary>
        [TestInitialize]
        public void InitializeFields()
        {
            HttpClient = new HttpClient();         
        }

        /// <summary>
        /// ��������� ������ �� ������������, ����� ���������� �� �����, ������ ��������� ������ '200 (OK)'
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
        /// ��������� ������ �� ������������, ����� ���������� �����, ������ ��������� ������ '400 (BadRequest)' �
        /// ������� 'Analizer is busy' 
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
        /// �������� �������� ������������, ����� ������ ��������� � �������� ������������, ������ ��������� ������ '200 (��)'
        /// </summary>
        /// <returns>200 (��)</returns>
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
        /// �������� �������� ������������, ����� ������ �� ��������� � �������� ������������, ������ ��������� ������ '400 (BadRequest)' �
        /// ������� 'Analyzer is not working'
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
        /// ��������� ������ �� ������������, ����� � ������� ���� ������, ����������� ��� ���������� �����������, ������ ��������� ������ '400 (BadReqeust)' �
        /// ������� 'Analyzer can not do this order.May be order contains services which analyzer does not support'
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
        /// ��������� ������ �� ������������, ����� � ������� ������ �� ������������ ����������, ������ �������� ������ '400 (BadRequest)' �
        /// ������� Analizer with name '{name}' not found
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
        /// �������� ��������� ������, ����� � ������� ������ �� ������������ ����������, ������ �������� ������ '400 (BadRequest)' �
        /// ������� Analizer with name '{name}' not found
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
