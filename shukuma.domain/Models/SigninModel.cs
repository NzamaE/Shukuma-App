using System.ComponentModel.DataAnnotations;

namespace shukuma.domain.Models
{
    public class SigninModel
    {
        [Required(ErrorMessage = "Please provide your email address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
