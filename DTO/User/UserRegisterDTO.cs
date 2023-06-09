using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementBE.DTO.User
{
    public class UserRegisterDTO
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,}$", ErrorMessage = "Password must include uppercase letters, numbers and special characters.")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email invalid.")]
        public string Email { get; set; } = string.Empty;
    }
}
