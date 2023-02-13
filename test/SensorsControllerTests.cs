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
    public class SensorsControllerTests
    {
        private HttpClient _httpClient;
        private static string accessToken = "";
        private static Guid? FarmId;
        private static Guid? FieldId;
        private static Guid? SensorId;
        private static string phone = "0963100004";
        public SensorsControllerTests()
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
            var stringResultVerify = await responseVerify.Content.ReadAsStringAsync();
            UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(stringResultVerify)!;
            Assert.IsTrue(userLogin.accessToken is string);
            SensorsControllerTests.accessToken = userLogin.accessToken;
        }
        [TestMethod]
        public async Task PostFarm()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            Farm farm = new Farm()
            {
                Name = "TName",
                Postcode = "1000",
                City = "TCity",
                Country = "TCountry"
            };
            HttpContent farmContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(farm), Encoding.UTF8, "application/json");
            var farmPostResponse = await _httpClient.PostAsync("/farms", farmContent);
            var stringResult = await farmPostResponse.Content.ReadAsStringAsync();
            Farm incomingFarm = JsonConvert.DeserializeObject<Farm>(stringResult)!;
            Assert.IsTrue(incomingFarm.FarmId is Guid);
            SensorsControllerTests.FarmId = incomingFarm.FarmId;
        }
        [TestMethod]
        public async Task PostField()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            Field field = new Field()
            {
                FarmId = (Guid)SensorsControllerTests.FarmId!,
                Name = "TName",
                Altitude = 80,
                Polygon = "TPolygon"
            };
            HttpContent fieldContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(field), Encoding.UTF8, "application/json");
            var fieldPostResponse = await _httpClient.PostAsync("/fields", fieldContent);
            var stringResult = await fieldPostResponse.Content.ReadAsStringAsync();
            Field incomingField = JsonConvert.DeserializeObject<Field>(stringResult)!;
            Assert.IsTrue(incomingField.FieldId is Guid);
            SensorsControllerTests.FieldId = incomingField.FieldId;
        }
        [TestMethod]
        public async Task PostSensor()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            SensorInput sensorInput = new SensorInput()
            {
                FieldId = (Guid)SensorsControllerTests.FieldId!,
                SerialNo = "TSerialNo",
                GDD = 320,
                DefaultGDD = 230,
                Long = 80,
                Lat = 80,
                InstallationDate = "12-2-2021",
                LastCuttingDate = "12-2-2021",
                BaseTemperature = 20,
            };
            HttpContent sensorContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(sensorInput), Encoding.UTF8, "application/json");
            var sensorInputPostResponse = await _httpClient.PostAsync("/sensors", sensorContent);
            var stringResult = await sensorInputPostResponse.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, sensorInputPostResponse.StatusCode);
            Sensor incomingSensor = JsonConvert.DeserializeObject<Sensor>(stringResult)!;
            Assert.IsTrue(incomingSensor.SensorId is Guid);
            SensorsControllerTests.SensorId = incomingSensor.SensorId;
        }
        [TestMethod]
        public async Task GetSensor_ReturnsValidSensor()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            var sensorResponse = await _httpClient.GetAsync("/sensors/" + SensorsControllerTests.SensorId);
            Assert.AreEqual(HttpStatusCode.OK, sensorResponse.StatusCode);
            var stringResult = await sensorResponse.Content.ReadAsStringAsync();
            Sensor sensor = JsonConvert.DeserializeObject<Sensor>(stringResult)!;
            Assert.IsTrue(sensor.SensorId == SensorsControllerTests.SensorId);
        }
        [TestMethod]
        public async Task PatchSensor_WithValidSensor_ReturnsNewSensor()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            SensorUpdate sensorUpdate = new SensorUpdate()
            {
                Long = 80,
                InstallationDate = "10-2-2021"
            };
            HttpContent sensorContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(sensorUpdate), Encoding.UTF8, "application/json");
            var sensorUpdateResponse = await _httpClient.PatchAsync("/sensors/" + SensorsControllerTests.SensorId, sensorContent);
            Assert.AreEqual(HttpStatusCode.OK, sensorUpdateResponse.StatusCode);
            var stringResult = await sensorUpdateResponse.Content.ReadAsStringAsync();
            Sensor sensor = JsonConvert.DeserializeObject<Sensor>(stringResult)!;
            Assert.IsTrue(sensor.InstallationDate == sensorUpdate.InstallationDate);
        }
        [TestMethod]
        public async Task GetSensorStatistics_ReturnsValidSensorStatistics()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            var response = await _httpClient.GetAsync("/sensors/" + SensorsControllerTests.SensorId + "/statistics");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            Sensor[] sensors = JsonConvert.DeserializeObject<Sensor[]>(stringResult)!;
            Assert.IsTrue(sensors[0].SensorId == SensorsControllerTests.SensorId);
        }
        [TestMethod]
        public async Task PostResetGdd_WithValidSensorGddResetInput_ReturnsSensorGddReset()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            SensorGddResetInput sensorUpdate = new SensorGddResetInput()
            {
                resetDate = DateTime.UtcNow.Date
            };
            HttpContent sensorContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(sensorUpdate), Encoding.UTF8, "application/json");
            var sensorUpdateResponse = await _httpClient.PostAsync("/sensors/" + SensorsControllerTests.SensorId + "/gdd-resets", sensorContent);
            Assert.AreEqual(HttpStatusCode.OK, sensorUpdateResponse.StatusCode);
            var stringResult = await sensorUpdateResponse.Content.ReadAsStringAsync();
            SensorGddReset sensor = JsonConvert.DeserializeObject<SensorGddReset>(stringResult)!;
            Assert.IsTrue(sensor.SensorId == SensorsControllerTests.SensorId);
        }

        [TestMethod]
        public async Task GetFarmNotifications_ReturnsValidFarmNotifications()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            var response = await _httpClient.GetAsync("/farms/" + SensorsControllerTests.FarmId + "/notifications");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            FarmNotification[] farmNotifications = JsonConvert.DeserializeObject<FarmNotification[]>(stringResult)!;
            Assert.IsTrue(farmNotifications.Length != 0);
        }
        [TestMethod]
        public async Task GetSensorResets_ReturnsValidSensorResets()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            var response = await _httpClient.GetAsync("/sensors/" + SensorsControllerTests.SensorId + "/gdd-resets");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            SensorGddReset[] sensor = JsonConvert.DeserializeObject<SensorGddReset[]>(stringResult)!;
            Assert.IsTrue(sensor[0].SensorId == SensorsControllerTests.SensorId);
        }
        [TestMethod]
        public async Task PostTrigger_ReturnsSensor()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            HttpContent sensorContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var sensorUpdateResponse = await _httpClient.PostAsync("/sensors/" + SensorsControllerTests.SensorId + "/trigger", sensorContent);
            Assert.AreEqual(HttpStatusCode.OK, sensorUpdateResponse.StatusCode);
            var stringResult = await sensorUpdateResponse.Content.ReadAsStringAsync();
            SensorGddReset sensor = JsonConvert.DeserializeObject<SensorGddReset>(stringResult)!;
            Assert.IsTrue(sensor.SensorId == SensorsControllerTests.SensorId);
        }
        [TestMethod]
        public async Task DeleteSensor_ReturnsValidSensor()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SensorsControllerTests.accessToken);
            var response = await _httpClient.DeleteAsync("/sensors/" + SensorsControllerTests.SensorId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResult = await response.Content.ReadAsStringAsync();
            Sensor sensor = JsonConvert.DeserializeObject<Sensor>(stringResult)!;
            Assert.IsTrue(sensor.SensorId == SensorsControllerTests.SensorId);
        }
    }
}