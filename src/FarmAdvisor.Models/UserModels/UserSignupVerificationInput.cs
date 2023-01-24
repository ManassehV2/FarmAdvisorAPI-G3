namespace FarmAdvisor.Models
{
    public class UserSignupVerificationInput
    {
        public string userSignVerificationToken { get; set; }  = null!;
        public string userSignVerificationCode { get; set; }  = null!;
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string passwordHash {get; set;} = null!;
    }
}
