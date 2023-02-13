using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace FarmAdvisor.Test
{
    [TestClass]
    public class FarmsControllerTests
    {
        private HttpClient _httpClient;

        public FarmsControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }
    }
}