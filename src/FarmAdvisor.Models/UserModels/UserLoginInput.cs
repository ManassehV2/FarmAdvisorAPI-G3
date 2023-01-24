namespace FarmAdvisor.Models
{
    public class UserLoginInput
    {
        public string phone { get; set; }  = null!;
        public string passwordHash { get; set; }  = null!;
    }
}
