using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

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
    public async Task WeatherApi_ReturnsInvalid_LatitudeForLatitude800()
    {
      var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=800&longitude=80&baseTemperature=20&currentGdd=40");
      var stringResult = await response.Content.ReadAsStringAsync();

      Assert.AreEqual("Invalid_Latitude", stringResult);
    }

    [TestMethod]
    public async Task WeatherApi_ReturnsInvalid_LongitudeForLongitude800()
    {
      var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=80&longitude=800&baseTemperature=20&currentGdd=40");
      var stringResult = await response.Content.ReadAsStringAsync();

      Assert.AreEqual("Invalid_Longitude", stringResult);
    }
  }
}