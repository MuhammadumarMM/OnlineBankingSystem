#region Usings
using Application.Common.Constants;
using Application.DTOs.Mappers;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

#endregion

namespace Web
{
    public static class Startup
    {
        private const string CORS_POLICY = "CorsPolicy";

        public static void AddDependencyInjection(this WebApplicationBuilder builder)
        {
            #region Default services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
            });
            #endregion

            #region DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));


            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            #endregion

            #region Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password requirements settings
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });
            #endregion

            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
            });
            #endregion
            #region Mapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            #endregion

            #region Application services

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddTransient<ICardInterface , CardRepository>();
            builder.Services.AddTransient<ICreditInterface, CreditRepository>();
            builder.Services.AddTransient<IDepositInterface, DepositRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<ITrancationInterface, TrancationRepository>();
            builder.Services.AddTransient<IBankCardInterface, BankCardRepository>();


            builder.Services.AddTransient<ICardService , CardService>();
            builder.Services.AddTransient<ICreditService , CreditService>();
            builder.Services.AddTransient<ITrancationService, TrancationService>();
            builder.Services.AddTransient<IBankCardService, BankCardService>();
            builder.Services.AddTransient<IDepositService , DepositService>();

            #endregion
        }

        public static void AddMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(CORS_POLICY);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            SeedDatabase(app);
        }

        private static void SeedDatabase(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            SeedDataAsync(context, userManager, roleManager).Wait();
        }

        private static async Task SeedDataAsync(ApplicationDbContext context,
                                                UserManager<ApplicationUser> userManager,
                                                RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = IdentityRoles.SUPER_ADMIN },
                new IdentityRole { Name = IdentityRoles.ADMIN },
                new IdentityRole { Name = IdentityRoles.USER }
            };

            var roleExists = roleManager.Roles.Any();
            if (!roleExists)
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var superAdmin = new ApplicationUser
            {
                FirstName = "Super",
                LastName = "Admin",
                Email = "superadmin@localhost",
                UserName = "superadmin@localhost"
            };

            var result = await userManager.CreateAsync(superAdmin, "Admin.123$");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, IdentityRoles.SUPER_ADMIN);
            }
        }
    }
}