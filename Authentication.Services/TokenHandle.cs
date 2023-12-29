using Authentication.Services.Abtract;
using Domain.Entity;
using Domain.ViewEntity.Authen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.Abtract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class TokenHandle:ITokenHandle
    {
        IConfiguration _configuration;
        IUserServices _userServices;
        IUserTokenServices _userTokenServices;
        public TokenHandle(IConfiguration configuration, IUserServices userServices, IUserTokenServices userTokenServices)
        {
            _configuration = configuration;
            _userServices = userServices;
            _userTokenServices = userTokenServices;
        }
        public async Task<(string,DateTime)> CreateToken(VUser user)
        {
            DateTime ExpDate = DateTime.Now.AddMinutes(int.Parse(_configuration["TokenBear:ExpTokenMinutes"]));

            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenBear:Isser"], ClaimValueTypes.String),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), ClaimValueTypes.String, _configuration["TokenBear:Isser"]),
                new Claim(JwtRegisteredClaimNames.Aud, "Kruskal Nguyen", ClaimValueTypes.String, _configuration["TokenBear:Audient"]),
                new Claim(JwtRegisteredClaimNames.Exp, ExpDate.ToString("yyyy/MM/dd hh:mm:ss"), ClaimValueTypes.String, _configuration["TokenBear:Isser"]),
                new Claim(ClaimTypes.Name, $"{user.LastName} {user.MiddleName} {user.FirstName}", ClaimValueTypes.String, _configuration["TokenBear:Isser"]),
                new Claim("Username", user.UserName, ClaimValueTypes.String, _configuration["TokenBear:Isser"]),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenBear:Issuer"],
                audience: _configuration["TokenBear:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: ExpDate,
                signingCredentials: credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);
            return await Task.FromResult((token, ExpDate));
        }
        public async Task<(string, DateTime, string)> CreateRefreshToken(VUser user)
        {
            DateTime ExpDate = DateTime.Now.AddHours(int.Parse(_configuration["TokenBear:ExpRefreshTokenHour"]));
            string serialNumber = Guid.NewGuid().ToString();

            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenBear:Isser"], ClaimValueTypes.String),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), ClaimValueTypes.String, _configuration["TokenBear:Isser"]),  
                new Claim(JwtRegisteredClaimNames.Exp, ExpDate.ToString("yyyy/MM/dd hh:mm:ss"), ClaimValueTypes.String, _configuration["TokenBear:Isser"]),
                new Claim(ClaimTypes.SerialNumber, serialNumber, ClaimValueTypes.String, _configuration["TokenBear:Isser"]),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenBear:Issuer"],
                audience: _configuration["TokenBear:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: ExpDate,
                signingCredentials: credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);
            return await Task.FromResult((token, ExpDate, serialNumber));
        }
        public async Task ValidateToken(TokenValidatedContext context)
        {
            var claims = context.Principal.Claims.ToList();

            if (claims.Count == 0)
            {
                context.Fail("There is no token content");
                return;
            }
                
            var identity = context.Principal.Identity as ClaimsIdentity;

            if(identity.FindFirst(JwtRegisteredClaimNames.Iss) is null)
            {
                context.Fail("Iss is no data");
                return;
            }

            if (identity.FindFirst("Username") is null)
            {
                context.Fail("User is no data");
                return;
            }

            if(!await _userServices.CheckExist(identity.FindFirst("Username").Value))
            {
                context.Fail("User does not exist");
                return;
            }

            if(identity.FindFirst(JwtRegisteredClaimNames.Exp) is null)
            {
                context.Fail("Exp is no data");
                return;
            }

            var dataExp = identity.FindFirst(JwtRegisteredClaimNames.Exp).Value;
            long ticks = long.Parse(dataExp);
            var date = DateTimeOffset.FromUnixTimeSeconds(ticks);
            var minutes = date.Subtract(DateTime.Now).TotalMinutes;

            if(minutes <= 0) {
                context.Fail("Token has expired");
                return;
            }
        }

        public async Task<Profile> ValidateRefreshToken(string refreshToken)
        {
            var cliamPriciple = new JwtSecurityTokenHandler().ValidateToken(
            refreshToken, new TokenValidationParameters
            {
                ValidIssuer = _configuration.GetSection("TokenBear:Isser").Value,
                ValidateIssuer = false,
                ValidAudience = _configuration.GetSection("TokenBear:Audient").Value,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("TokenBear:SignatureKey").Value)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            },
            out _
            );

            if (cliamPriciple is null) return null;

            string serialNumber = cliamPriciple.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;

            UserToken token = await _userTokenServices.GetUserToken(serialNumber);

            if (token is null) return null;

            Profile profile = new Profile();

            profile.BaseInfo = await _userServices.GetUser(token.UserId);

            (profile.Token, _) = await CreateToken(profile.BaseInfo);
            (profile.RefreshToken, _,_)  = await CreateRefreshToken(profile.BaseInfo);

            return profile;
        }
    }
    
}
