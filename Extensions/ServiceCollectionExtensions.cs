using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Auth.Repositories;
using Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Auth.Validators;
using Auth.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration["DatabaseUrl"];
            services.AddDbContext<Auth.Models.DbContext>(options =>
            options.UseSqlServer(connection));
            services.AddIdentity<User, IdentityRole<long>>()
                .AddEntityFrameworkStores<Auth.Models.DbContext>()
                .AddDefaultTokenProviders();
        }
        public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AuthorizationSettings");
            services.Configure<AuthorizationSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AuthorizationSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                        var userId = context.Principal.Identity.Name;
                        var user = await userService.Users.FirstOrDefaultAsync(y => y.Id.ToString() == userId && y.IsActive);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auth",
                    Version = "v1",
                    Description = "API for auth features",
                    Contact = new OpenApiContact() { Name = "Mateusz Szczęsny", Email = "mateusz@mszczesny.com" }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAuthRepository, AuthRepository>();
        }
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
        }
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<LoginRequestValidator>();
            services.AddTransient<SignUpRequestValidator>();
        }
    }
}