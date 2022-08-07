using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Utils;

namespace FinalPorject.Extensions
{
    public static class JwtAuthenticationSeriveExtension
    {
        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var secretKey = Encoding.UTF8.GetBytes(configuration["AuthenticationOptions:SecretKey"]);

                    var validationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateAudience = true,
                        ValidAudience = configuration["AuthenticationOptions:Audience"],

                        ValidateIssuer = true,
                        ValidIssuer = configuration["AuthenticationOptions:Issuer"]
                    };

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = validationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context => throw new AppException(message: "Authentication failed(OnAuthenticationFailed).", statusCode: 401),
                        OnChallenge = context => throw new AppException(message: "Authentication failed(OnChallenge).", statusCode: 401),
                        OnForbidden = context => throw new AppException(message: "Permission denied(OnForbidden).", statusCode: 403),
                        OnMessageReceived = context => Task.CompletedTask,
                        OnTokenValidated = context => Task.CompletedTask,
                    };
                });
        }
    }
}
