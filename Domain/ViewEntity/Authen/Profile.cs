using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewEntity.Authen
{
    public class Profile
    {
        public Profile()
        {
        }

        public VUser BaseInfo { get; set; }
        public string? Token {  get; set; }
        public string? RefreshToken { get; set;}
    }
}
