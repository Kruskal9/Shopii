using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }
    }
}
