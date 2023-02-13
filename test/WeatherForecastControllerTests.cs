using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FarmAdvisor.Models;
using Newtonsoft.Json;
namespace FarmAdvisor.Test
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        private HttpClient _httpClient;
        public WeatherForecastControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }
        [TestMethod]
        public async Task WeatherApi_WithLatitude800_ReturnsInvalid_Latitude()
        {
            var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=800&longitude=80&baseTemperature=20&currentGdd=40");
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Latitude", stringResult);
        }
        [TestMethod]
        public async Task WeatherApi_WithLongitude800_ReturnsInvalid_Longitude()
        {
            var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=80&longitude=800&baseTemperature=20&currentGdd=40");
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Longitude", stringResult);
        }
        [TestMethod]
        public async Task WeatherApi_ReturnsListOfOneDayWeatherForecast()
        {
            var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=80&longitude=80&baseTemperature=20&currentGdd=40");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            OneDayWeatherForecast[] WeatherForecast = JsonConvert.DeserializeObject<OneDayWeatherForecast[]>(stringResult)!;
            Assert.IsTrue(WeatherForecast[0].Date is string);
        }
    }
}