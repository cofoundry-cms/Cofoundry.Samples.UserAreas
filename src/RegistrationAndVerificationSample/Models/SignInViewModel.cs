using System.ComponentModel.DataAnnotations;

namespace RegistrationAndVerificationSample
{
    public class SignInViewModel
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please provide your password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}