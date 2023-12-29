using Domain.Entity;
using Domain.ViewEntity.Authen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services.Abtract
{
    public interface ITokenHandle
    {
        Task<(string, DateTime, string)> CreateRefreshToken(VUser user);
        Task<(string, DateTime)> CreateToken(VUser user);
        Task<Profile> ValidateRefreshToken(string refreshToken);
        Task ValidateToken(TokenValidatedContext context);
    }
}
