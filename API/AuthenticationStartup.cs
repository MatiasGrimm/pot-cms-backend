using PotShop.API.Auth;
using PotShop.API.Data;
using PotShop.API.Helpers;
using PotShop.API.Models;
using PotShop.API.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace PotShop.API
{
    public static class AuthenticationStartup
    {
        private const string SecretKey = "A29bP9sY4ROddXAX9LyBSV9EIUOYRFlQdJ8e16OajUVEhwOcOSc5ibW0i/Prl9DVko4daUG6/b8WCso8+glwAg==";
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));

        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();

            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // define authorization policies;
            // specify what roles are allowed to access each policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.RequireAdmin, policy => policy.RequireClaim(AuthConstants.JwtClaimIdentifiers.Rol, AuthRoles.Admin));
                options.AddPolicy(AuthPolicies.RequireManager, policy => policy.RequireClaim(AuthConstants.JwtClaimIdentifiers.Rol, AuthRoles.Admin, AuthRoles.Manager));
            });

            // add identity
            var builder = services.AddIdentityCore<ApiUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApiDbContext>().AddDefaultTokenProviders();
            builder.AddRoles<IdentityRole>();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")));
        }
    }
}
