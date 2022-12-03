using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using WireMock.Server;
namespace FarmAdvisor.Common.Test
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly WireMockServer _wireMockServer;
        public string WireMockUrl => _wireMockServer.Urls.First();


        public TestWebApplicationFactory()
        {
            _wireMockServer = WireMockServer.Start();
            BuildConfiguration();
        }

        private static void BuildConfiguration()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }
        public TestWebApplicationFactory<TStartup> WithEnvironment(Dictionary<string, string> variables)
        {
            foreach (var variable in variables)
            {
                Environment.SetEnvironmentVariable(variable.Key, variable.Value);
            }

            return this;
        }
        public TestWebApplicationFactory<TStartup> WithRemoteServerConfig(Action<WireMockServer> action)
        {
            action.Invoke(_wireMockServer);
            return this;
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
            });
        }
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(hostbuilder =>
                {
                    hostbuilder.UseStartup<TStartup>();
                }).ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddEnvironmentVariables();
                });
        }
    }

}