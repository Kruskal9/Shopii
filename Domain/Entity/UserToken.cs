using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class UserToken:IdentityUserToken<string>
    {
        public UserToken()
        {
        }

        public string RefreshToken {  get; set; }
        public DateTime ExpToken { get; set; }
        public DateTime ExpRefreshToken { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
