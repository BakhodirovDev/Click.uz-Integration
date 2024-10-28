using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities.Users
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public float Balance { get; set; } = 0;
        public string HashPassword { get; set; }

    }
}
