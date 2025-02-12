using System.ComponentModel.DataAnnotations;

namespace Clean_Life_API.DTO
{
    public class ForgetPasswordDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}

