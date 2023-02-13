// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using System.Net.Http;
// using System.Threading.Tasks;
// using FarmAdvisor.Models;

// namespace FarmAdvisor.Test
// {
//     [TestClass]
//     public class UsersControllerTests
//     {
//         private HttpClient _httpClient;

//         public UsersControllerTests()
//         {
//             var webAppFactory = new WebApplicationFactory<Program>();
//             _httpClient = webAppFactory.CreateDefaultClient();
//         }
//         [TestMethod]
//         public async Task UsersSignUp_InvalidPhone_ReturnsInvalid_Phone()
//         {
//             UserSignup userSignup = new UserSignup()
//             {
//                 phone = "invalid"
//             };
//             var response = await _httpClient.PostAsync("/users/signup", userSignup);
//             var stringResult = await response.Content.ReadAsStringAsync();

//             Assert.AreEqual("Invalid_Longitude", stringResult);
//         }
//     }
// }