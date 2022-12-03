using FarmAdvisor.HttpFunctions;
using FarmAdvisor.HttpFunctions.Functions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FarmAdvisor.Functions.Test.HttpFunctionsTest.Infrastructure
{
    public class HttpFunctionsTestStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<FarmFieldFunctions>();
            HttpFunctionStartup.ConfigureServices(services);
        }
    }
}
