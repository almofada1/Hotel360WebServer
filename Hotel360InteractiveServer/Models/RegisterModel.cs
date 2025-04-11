using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hotel360InteractiveServer.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Retyping the password is required.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
