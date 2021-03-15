using System;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hermes.API.User.Domain.Data;
using Hermes.API.User.Domain.Entities;
using Hermes.API.User.Domain.Filters;
using Hermes.API.User.Domain.Mappers;
using Hermes.API.User.Domain.Responses;
using Hermes.API.User.Domain.Services;
using Hermes.API.User.Domain.Utils;
using Hermes.API.User.Domain.Validators;
using Hermes.API.User.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Hermes.API.User
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddFluentValidation(o =>
                    {
                        o.RegisterValidatorsFromAssemblyContaining<ChangePasswordValidator>();
                        o.RegisterValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();
                        o.RegisterValidatorsFromAssemblyContaining<UserUpdateRequestValidator>();
                    }
                )
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; }
                )
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                            .SelectMany(v => v.Errors)
                            .Select(v => v.ErrorMessage)
                            .ToList();
                        return new BadRequestObjectResult(new ValidationErrorResponse
                        {
                            Errors = errors
                        });
                    };
                });

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Hermes User API", Version = "v1"});
            });

            // Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Mappers

            services.AddAutoMapper(typeof(MappingProfile));


            // Authentication
            RegisterIdentity(services);
            RegisterAuth(services);

            // Server Configuration
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddConsulConfig(Configuration);
            services.AddHealthChecks();
            services.AddMiniProfiler(options => { options.RouteBasePath = "/profiles"; })
                .AddEntityFramework();
        }

        private void RegisterAuth(IServiceCollection services)
        {
            var jwtConfig = new JwtOptions();
            Configuration.Bind("JwtAuth", jwtConfig);
            services.AddSingleton(jwtConfig);

            var key = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.RefreshOnIssuerKeyNotFound = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization(authorizationOptions =>
            {
                var defaultPoliceBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultPoliceBuilder = defaultPoliceBuilder.RequireAuthenticatedUser();
                authorizationOptions.DefaultPolicy = defaultPoliceBuilder.Build();
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private void RegisterIdentity(IServiceCollection services)
        {
            services.AddIdentityCore<HermesUser>(options =>
                {
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@._";
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddRoles<HermesRole>()
                .AddSignInManager()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<HermesIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<SecurityStampValidatorOptions>(
                options => { options.ValidationInterval = TimeSpan.Zero; });
            services.AddSingleton<IPasswordHasher<HermesUser>, PasswordHasher<HermesUser>>();

            var connectionString = Configuration.GetValue<string>("HermesIdentityConnectionString");
            services
                .AddDbContext<HermesIdentityDbContext>(options => { options.UseSqlServer(connectionString); });

            services.AddValidatorsFromAssemblyContaining(typeof(UserRegisterRequestValidator));
            var jwtOptions = new JwtOptions();
            Configuration.Bind("JwtAuth", jwtOptions);
            services.AddSingleton(jwtOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiniProfiler();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["ApplicationName"]); });

            app.UseHealthChecks("/healthcheck");

            app.UseConsul();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");

            app.UseRewriter(option);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}