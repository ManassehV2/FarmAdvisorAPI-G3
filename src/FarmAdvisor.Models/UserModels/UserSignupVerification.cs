namespace FarmAdvisor.Models
{
    public class UserSignupVerification
    {
        public string userSignVerificationToken { get; set; }
        public string? userSignVerificationCode { get; set; }
        public UserSignupVerification(string verificationToken, string? verificationCode = null){
            this.userSignVerificationToken = verificationToken;
            this.userSignVerificationCode = verificationCode;
        }
    }
}
