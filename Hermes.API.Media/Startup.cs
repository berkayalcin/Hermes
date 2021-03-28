using System.Collections.Generic;
using System.Linq;
using FluentValidation.AspNetCore;
using Hermes.API.Media.Domain.Authentication;
using Hermes.API.Media.Domain.Filters;
using Hermes.API.Media.Domain.Responses;
using Hermes.API.Media.Domain.Services;
using Hermes.API.Media.Domain.Validators;
using Hermes.API.Media.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Hermes.API.Media
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
                .AddFluentValidation(o => { o.RegisterValidatorsFromAssemblyContaining<UploadImageRequestValidator>(); }
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
            AddSwagger(services);

            // Auth
            RegisterJwtOptions(services);

            // Server Config
            services.AddConsulConfig(Configuration);
            services.AddHealthChecks();

            // Mappers


            // Services
            services.AddScoped<IImageService, ImageService>();


            // Repositories


            // Elastic Search
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
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
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Hermes Media API", Version = "v1"});
            });
        }

        private void RegisterJwtOptions(IServiceCollection services)
        {
            var jwtConfig = new JwtOptions();
            Configuration.Bind("JwtAuth", jwtConfig);
            services.AddSingleton(jwtConfig);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiniProfiler();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["ApplicationName"]); });

            app.UseHealthChecks("/healthcheck");

            app.UseConsul();

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