using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryCodeFirstCore.Data
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; } = new Byte[64];
        [Required]
        public byte[] PasswordSalt { get; set; } = new Byte[64];
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength (20)]
        public string Role { get; set; } = string.Empty;
    }
}
