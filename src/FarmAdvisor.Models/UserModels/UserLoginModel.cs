
namespace FarmAdvisor.Models
{
    public class UserLogin
    {
        public Guid? userId { get; set; } = null!;
        public string name { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string email { get; set; } = null!;
        public string accessToken { get; set; } = null!;
    }
}