using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using FarmAdvisor.Models;
using System.Text;
using Newtonsoft.Json;
using System;
namespace FarmAdvisor.Test
{
    [TestClass]
    public class UsersControllerTests
    {
        private HttpClient _httpClient;
        private static string userSignupVerificationToken = "";
        private static string accessToken = "";
        private static string phone = "0963200000";
        public UsersControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }
        [TestMethod]
        public async Task UsersSignUp_InvalidPhone_ReturnsInvalid_Phone()
        {
            UserSignup userSignup = new UserSignup()
            {
                phone = "invalid"
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignup), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Phone", stringResult);
        }
        [TestMethod]
        public async Task UsersSignUp_ValidPhone_ReturnsUserSignVerificationToken()
        {
            UserSignup userSignup = new UserSignup()
            {
                phone = phone
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignup), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            UserSignupVerification userSignupVerification = JsonConvert.DeserializeObject<UserSignupVerification>(stringResult)!;
            Assert.IsTrue(userSignupVerification.userSignVerificationToken is string);
            UsersControllerTests.userSignupVerificationToken = userSignupVerification.userSignVerificationToken;
        }
        [TestMethod]
        public async Task UsersSignUpVerify_InvalidToken_ReturnsInvalid_Token()
        {
            UserSignupVerificationInput userSignupVerificationInput = new UserSignupVerificationInput()
            {
                userSignVerificationToken = "invalid",
                userSignVerificationCode = "123456",
                name = "Test Name",
                email = "test@gmail.com",
                passwordHash = "pwh"
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignupVerificationInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup/verify", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Token", stringResult);
        }
        [TestMethod]
        public async Task UsersSignUpVerify_InvalidVerificationCode_ReturnsInvalid_Verification_Code()
        {
            UserSignupVerificationInput userSignupVerificationInput = new UserSignupVerificationInput()
            {
                userSignVerificationToken = UsersControllerTests.userSignupVerificationToken,
                userSignVerificationCode = "invalid",
                name = "Test Name",
                email = "test@gmail.com",
                passwordHash = "pwh"
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignupVerificationInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup/verify", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Verification_Code", stringResult);
        }
        [TestMethod]
        public async Task UsersSignUpVerify_InvalidEmail_ReturnsInvalid_Email()
        {
            UserSignupVerificationInput userSignupVerificationInput = new UserSignupVerificationInput()
            {
                userSignVerificationToken = UsersControllerTests.userSignupVerificationToken,
                userSignVerificationCode = "123456",
                name = "Test Name",
                email = "test",
                passwordHash = "pwh"
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignupVerificationInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup/verify", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Email", stringResult);
        }
        [TestMethod]
        public async Task UsersSignUpVerify_ValidUserSignupVerificationInput_ReturnsValid_UserLogin()
        {
            UserSignupVerificationInput userSignupVerificationInput = new UserSignupVerificationInput()
            {
                userSignVerificationToken = UsersControllerTests.userSignupVerificationToken,
                userSignVerificationCode = "123456",
                name = "Test Name",
                email = "test@gmail.com",
                passwordHash = "pwh"
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignupVerificationInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup/verify", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(stringResult)!;
            
            Assert.IsTrue(userLogin.accessToken is string);
            Assert.IsTrue(userLogin.phone == phone);
        }
        [TestMethod]
        public async Task UsersSignUpVerify_ExistingPhone_ReturnsPhone_Already_Exists()
        {
            UserSignupVerificationInput userSignupVerificationInput = new UserSignupVerificationInput()
            {
                userSignVerificationToken = UsersControllerTests.userSignupVerificationToken,
                userSignVerificationCode = "123456",
                name = "Test Name",
                email = "test@gmail.com",
                passwordHash = "pwh"
            };
            HttpContent userSignupContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userSignupVerificationInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/signup/verify", userSignupContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Phone_Already_Exists", stringResult);
        }
        [TestMethod]
        public async Task UsersLogin_InvalidPhone_ReturnsInvalid_Phone()
        {
            UserLoginInput userLoginInput = new UserLoginInput()
            {
                phone = "invalid",
                passwordHash = "pwh"
            };
            HttpContent userLoginInputContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userLoginInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/login", userLoginInputContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Invalid_Phone", stringResult);
        }
        [TestMethod]
        public async Task UsersLogin_ValidLoginInput_ReturnsValidUserLogIn()
        {
            UserLoginInput userLoginInput = new UserLoginInput()
            {
                phone = phone,
                passwordHash = "pwh"
            };
            HttpContent userLoginInputContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(userLoginInput), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/users/login", userLoginInputContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(stringResult)!;
            Assert.IsTrue(userLogin.accessToken is string);
            Assert.IsTrue(userLogin.phone == phone);
            UsersControllerTests.accessToken = userLogin.accessToken;
        }
        [TestMethod]
        public async Task UsersMe_ValidAccessToken_ReturnsValidUser()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + UsersControllerTests.accessToken);
            var response = await _httpClient.GetAsync("/users/me");
            var stringResult = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(stringResult)!;
            Assert.IsTrue(user.Phone == phone);
        }
    }
}