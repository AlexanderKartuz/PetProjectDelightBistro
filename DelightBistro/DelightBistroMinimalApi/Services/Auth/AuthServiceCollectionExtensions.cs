using DelightBistroMinimalApi.Services.Auth.Interfaces;
using DelightBistroMinimalApi.Services.Auth.Options;
using DelightBistroMvc.Data;
using DelightBistroMvc.Data.Repositories;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.PasswordHasher;
using DelightBistroMvc.Data.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DelightBistroMinimalApi.Services.Auth
{
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddDelightBistroJwtAuth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT не настроен.");

            if (string.IsNullOrEmpty(jwt.Key) || jwt.Key.Length < 32)
            {
                throw new InvalidOperationException("JWT: Key должен быть больше 32 символов.");
            }

            var userConnection = configuration.GetConnectionString("Users")
                ?? throw new InvalidOperationException("ConnectionString: Users обязателен для JWT login.");

            services.AddDbContext<WebContext>(op => op.UseSqlServer(userConnection));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                        ClockSkew = TimeSpan.FromMinutes(1),
                    };
                });

            services.AddAuthorization();

            return services;
        }

        /// <summary>
        /// Кнопка Authorize в Swagger для Bearer.
        /// </summary>
        public static IServiceCollection AddDelightBistroSwaggerWithJwt(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DelightBistro API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT: вставьте токен (без слова Bearer) или целиком Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }
    }
}
