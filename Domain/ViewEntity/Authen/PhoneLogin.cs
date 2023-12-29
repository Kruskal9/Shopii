using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewEntity.Authen
{
    public class PhoneLogin : BaseAccount
    {
        [Required]
        [Phone]
        public required string NumberPhone { get; set; }
    }
}
