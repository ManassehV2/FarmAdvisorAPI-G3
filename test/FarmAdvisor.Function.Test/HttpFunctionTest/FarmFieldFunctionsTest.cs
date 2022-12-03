using System.Text;
using System.Text.Json;
using FarmAdvisor.Functions.Test.HttpFunctionsTest.Infrastructure;
using FarmAdvisor.HttpFunctions.Functions;
using FarmAdvisor.Models.Models;
using FarmAdvisor.Common.Test;
using FarmAdvisor.Common.Test.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FarmAdvisor.Functions.Test.HttpFunctionsTest
{
    public class FarmFieldFunctionsTest : IClassFixture<TestWebApplicationFactory<HttpFunctionsTestStartup>>
    {
        private readonly IServiceProvider _serviceProvider;

        public FarmFieldFunctionsTest(TestWebApplicationFactory<HttpFunctionsTestStartup> factory)
        {
            _serviceProvider = factory.WithEnvironment(new Dictionary<string, string>
            {
                { "AzureWebJobsStorage", "UseDevelopmentStorage=true" },
                { "SveveApiUserOptions:BaseUrl", factory.WireMockUrl },
                { "SveveApiUserOptions:UserName","testuser" },
                { "SveveApiUserOptions:Pwd","testpwd" },
                { "MetWeatherAPIBaseUrl", factory.WireMockUrl }

            }).Services;

        }
        [Fact]
        public async Task FarmFieldDashboard_WithValidFieldId_ReturnsOK()
        {
            //Arrange
            var functionClass = _serviceProvider.GetService<FarmFieldFunctions>();
            var testFarmId = Guid.NewGuid();

            //Act
            var response = (OkObjectResult)await functionClass.GetAllFarmFields(FakeHttpRequest.Create($"https://localhost/users/farms/fields?farmId={testFarmId}"));

            //Assert
            Assert.Equal(200, response.StatusCode);

        }

    }
}