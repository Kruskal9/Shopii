using Authentication.Services.Abtract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Configuation
{
    public static class ConfigTokenService
    {
        public static void RegisterToken(this IServiceCollection service, IConfiguration config)
        {
            service.AddAuthentication(option =>
            {
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = config.GetSection("TokenBear:Isser").Value,
                        ValidateIssuer = false,
                        ValidAudience = config.GetSection("TokenBear:Audient").Value,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("TokenBear:SignatureKey").Value)),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    option.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var tokenHandle = context.HttpContext.RequestServices.GetRequiredService<ITokenHandle>();
                            return tokenHandle.ValidateToken(context);
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var exception = context.Exception;
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            var request = context.Request;
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
