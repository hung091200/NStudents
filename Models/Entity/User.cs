using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NStudents.Models.Entity
{
    [Table("users")]
    public class User
    {
        internal string password;

        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Role { get; set; } = "Student";

        [ForeignKey("Student")]
        public int? StudentId { get; set; }

        public Students? Student { get; set; } = null!;
    }
}
