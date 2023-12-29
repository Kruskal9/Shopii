using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewEntity.Authen
{
    public class BaseAccount
    {
        [Required]
        [MaxLength(100)]
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
