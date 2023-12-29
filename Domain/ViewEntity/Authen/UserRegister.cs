using Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewEntity.Authen
{
    public class UserRegister : BaseAccount
    {
        public required VUser VUser { get; set; }
        [Required]
        [MaxLength(100)]
        [PasswordPropertyText]
        public required string RePassword { get; set; }
    }
}
