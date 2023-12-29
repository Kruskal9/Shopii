using Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewEntity.Authen
{
    public class VUser
    {
        public VUser()
        {
        }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string NumberPhone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }

        public User MapToUser()
        { 
            return new User
            {
                UserName = UserName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                PhoneNumber = NumberPhone,
                Email = Email,
            };
        }

        public VUser MapToThis(User user)
        {
            return new VUser {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                NumberPhone = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}
