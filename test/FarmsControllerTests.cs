using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using FarmAdvisor.Models;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Net;

namespace FarmAdvisor.Test
{
    [TestClass]
    public class FarmsControllerTests
    {
        private HttpClient _httpClient;
        private static string accessToken = "";
        private static Guid? FarmId;
        private static string phone = "0963000000";
        public FarmsControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }
        [TestMethod]
        public async Task UsersSignUp()
        {
            UserSignup userSignup = new UserSignup()
            {
                phone = phone
            };
            HttpContent userSignupContentSignup = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignup), Encoding.UTF8, "application/json");

            var responseSignup = await _httpClient.PostAsync("/users/signup", userSignupContentSignup);
            var stringResultSignup = await responseSignup.Content.ReadAsStringAsync();
            UserSignupVerification userSignupVerification = JsonConvert.DeserializeObject<UserSignupVerification>(stringResultSignup)!;
            UserSignupVerificationInput userSignupVerificationInput = new UserSignupVerificationInput()
            {
                userSignVerificationToken = userSignupVerification.userSignVerificationToken,
                userSignVerificationCode = "123456",
                name = "Test Name",
                email = "test@gmail.com",
                passwordHash = "pwh"
            };
            HttpContent userSignupContentVerify = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignupVerificationInput), Encoding.UTF8, "application/json");

            var responseVerify = await _httpClient.PostAsync("/users/signup/verify", userSignupContentVerify);
            Assert.AreEqual(HttpStatusCode.OK, responseVerify.StatusCode);
            var stringResultVerify = await responseVerify.Content.ReadAsStringAsync();
            UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(stringResultVerify)!;

            Assert.IsTrue(userLogin.accessToken is string);

            FarmsControllerTests.accessToken = userLogin.accessToken;
        }


        [TestMethod]
        public async Task PostFarm_WithValidFarm_ReturnsNewFarm()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FarmsControllerTests.accessToken);
            Farm farm = new Farm()
            {
                Name = "TName",
                Postcode = "1000",
                City = "TCity",
                Country = "TCountry"
            };
            HttpContent farmContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(farm), Encoding.UTF8, "application/json");

            var farmPostResponse = await _httpClient.PostAsync("/farms", farmContent);
            Assert.AreEqual(HttpStatusCode.OK, farmPostResponse.StatusCode);
            var stringResult = await farmPostResponse.Content.ReadAsStringAsync();
            Farm incomingFarm = JsonConvert.DeserializeObject<Farm>(stringResult)!;

            Assert.IsTrue(incomingFarm.FarmId is Guid);

            FarmsControllerTests.FarmId = incomingFarm.FarmId;
        }

        [TestMethod]
        public async Task GetFarms_ReturnsFarmsList()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FarmsControllerTests.accessToken);
            var farmsResponse = await _httpClient.GetAsync("/farms");

            Assert.AreEqual(HttpStatusCode.OK, farmsResponse.StatusCode);
            var stringResult = await farmsResponse.Content.ReadAsStringAsync();
            Farm[] farms = JsonConvert.DeserializeObject<Farm[]>(stringResult)!;
            Assert.IsTrue(farms[0].FarmId == FarmsControllerTests.FarmId);
        }

        [TestMethod]
        public async Task GetFarm_ReturnsValidFarm()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FarmsControllerTests.accessToken);
            var farmResponse = await _httpClient.GetAsync("/farms/" + FarmsControllerTests.FarmId);

            Assert.AreEqual(HttpStatusCode.OK, farmResponse.StatusCode);
            var stringResult = await farmResponse.Content.ReadAsStringAsync();
            Farm farm = JsonConvert.DeserializeObject<Farm>(stringResult)!;
            Assert.IsTrue(farm.FarmId == FarmsControllerTests.FarmId);
        }


        [TestMethod]
        public async Task PatchFarm_WithValidFarm_ReturnsNewFarm()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FarmsControllerTests.accessToken);
            FarmUpdate farmUpdate = new FarmUpdate()
            {
                Name = "updatedTName",
                Postcode = "1000",
                City = "updatedTCity",
                Country = "updatedTCountry"
            };

            HttpContent farmContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(farmUpdate), Encoding.UTF8, "application/json");
            var farmUpdateResponse = await _httpClient.PatchAsync("/farms/" + FarmsControllerTests.FarmId, farmContent);

            Assert.AreEqual(HttpStatusCode.OK, farmUpdateResponse.StatusCode);
            var stringResult = await farmUpdateResponse.Content.ReadAsStringAsync();
            Farm farm = JsonConvert.DeserializeObject<Farm>(stringResult)!;
            Assert.IsTrue(farm.Name == farmUpdate.Name);
        }

        [TestMethod]
        public async Task GetFarmNotifications_ReturnsValidFarmNotifications()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FarmsControllerTests.accessToken);
            var response = await _httpClient.GetAsync("/farms/" + FarmsControllerTests.FarmId + "/notifications");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            FarmNotification[] farmNotifications = JsonConvert.DeserializeObject<FarmNotification[]>(stringResult)!;
            Assert.IsTrue(farmNotifications.Length == 0);
        }
    }
}