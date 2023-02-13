// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using System.Net.Http;
// using System.Threading.Tasks;

// namespace FarmAdvisor.Test
// {
//   [TestClass]
//   public class ApiTests
//   {
//     private HttpClient _httpClient;

//     public ApiTests()
//     {
//       var webAppFactory = new WebApplicationFactory<Program>();
//       _httpClient = webAppFactory.CreateDefaultClient();
//     }

//     [TestMethod]
//     public async Task DefaultRoute_ReturnsAllGood()
//     {
//       var response = await _httpClient.GetAsync("");
//       var stringResult = await response.Content.ReadAsStringAsync();

//       Assert.AreEqual("All Good!", stringResult);
//     }



//     // [TestMethod]
//     // public async Task WeatherApi_ReturnsUser_Not_FoundForNoUser()
//     // {
//     //   var response = await _httpClient.GetAsync("/farms");
//     //   var stringResult = await response.Content.ReadAsStringAsync();

//     //   Assert.AreEqual("User_Not_Found", stringResult);
//     // }

//     [TestMethod]
//     public async Task WeatherApi_ReturnsInvalid_LatitudeForLatitude800()
//     {
//       var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=800&longitude=80&baseTemperature=20&currentGdd=40");
//       var stringResult = await response.Content.ReadAsStringAsync();

//       Assert.AreEqual("Invalid_Latitude", stringResult);
//     }

//     [TestMethod]
//     public async Task WeatherApi_ReturnsInvalid_LongitudeForLongitude800()
//     {
//       var response = await _httpClient.GetAsync("/weather-api/forecast?altitude=80&latitude=80&longitude=800&baseTemperature=20&currentGdd=40");
//       var stringResult = await response.Content.ReadAsStringAsync();

//       Assert.AreEqual("Invalid_Longitude", stringResult);
//     }

//     // [TestMethod]
//     // public async Task Sum_Returns16For10And6()
//     // {
//     //   var response = await _httpClient.GetAsync("/sum?n1=10&n2=6");
//     //   var stringResult = await response.Content.ReadAsStringAsync();
//     //   var intResult = int.Parse(stringResult);

//     //   Assert.AreEqual(16, intResult);
//     // }
//   }
// }