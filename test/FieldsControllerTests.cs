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
    public class FieldsControllerTests
    {
        private HttpClient _httpClient;
        private static string accessToken = "";
        private static Guid? FarmId;
        private static Guid? FieldId;
        private static string phone = "0963400000";
        public FieldsControllerTests()
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

            FieldsControllerTests.accessToken = userLogin.accessToken;
        }


        [TestMethod]
        public async Task PostFarm_WithValidFarm_ReturnsNewFarm()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FieldsControllerTests.accessToken);
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

            FieldsControllerTests.FarmId = incomingFarm.FarmId;
        }


        [TestMethod]
        public async Task PostField()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FieldsControllerTests.accessToken);
            Field field = new Field()
            {
                FarmId = (Guid)FieldsControllerTests.FarmId!,
                Name = "TName",
                Altitude = 80,
                Polygon = "TPolygon"
            };

            HttpContent fieldContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(field), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/fields", fieldContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            Field incomingField = JsonConvert.DeserializeObject<Field>(stringResult)!;
            Assert.IsTrue(incomingField.FieldId is Guid);

            FieldsControllerTests.FieldId = incomingField.FieldId;
        }

        [TestMethod]
        public async Task GetField_ReturnsValidField()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FieldsControllerTests.accessToken);
            var fieldResponse = await _httpClient.GetAsync("/fields/" + FieldsControllerTests.FieldId);

            Assert.AreEqual(HttpStatusCode.OK, fieldResponse.StatusCode);
            var stringResult = await fieldResponse.Content.ReadAsStringAsync();
            Field field = JsonConvert.DeserializeObject<Field>(stringResult)!;
            Assert.IsTrue(field.FieldId == FieldsControllerTests.FieldId);
        }


        [TestMethod]
        public async Task PatchField_WithValidField_ReturnsNewField()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FieldsControllerTests.accessToken);
            FieldUpdate fieldUpdate = new FieldUpdate()
            {
                Name = "updatedTName",
                Altitude = 80,
                Polygon = "updatedPolygon"
            };

            HttpContent fieldContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(fieldUpdate), Encoding.UTF8, "application/json");
            var fieldUpdateResponse = await _httpClient.PatchAsync("/fields/" + FieldsControllerTests.FieldId, fieldContent);

            Assert.AreEqual(HttpStatusCode.OK, fieldUpdateResponse.StatusCode);
            var stringResult = await fieldUpdateResponse.Content.ReadAsStringAsync();
            Field field = JsonConvert.DeserializeObject<Field>(stringResult)!;
            Assert.IsTrue(field.Name == fieldUpdate.Name);
        }

        [TestMethod]
        public async Task GetFieldSensors_ReturnsValidFieldSensors()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + FieldsControllerTests.accessToken);
            var response = await _httpClient.GetAsync("/fields/" + FieldsControllerTests.FieldId + "/sensors");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            Sensor[] sensors = JsonConvert.DeserializeObject<Sensor[]>(stringResult)!;
            Assert.IsTrue(sensors.Length == 0);
        }
    }
}