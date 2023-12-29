using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewEntity.Authen
{
    public class EmailLogin : BaseAccount
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
