using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couchbase.Configuration.Client;
using Couchbase.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Hermes.API.Advertisement.Domain.Authentication;
using Hermes.API.Advertisement.Domain.Filters;
using Hermes.API.Advertisement.Domain.Mappers;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Responses;
using Hermes.API.Advertisement.Domain.Services.Advertisement;
using Hermes.API.Advertisement.Domain.Services.AdvertisementBucketProvider;
using Hermes.API.Advertisement.Domain.Validators;
using Hermes.API.Advertisement.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Hermes.API.Advertisement
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
                        o.RegisterValidatorsFromAssemblyContaining<CreateAdvertisementRequestValidator>();
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
            AddSwagger(services);

            // Auth
            RegisterJwtOptions(services);

            // Server Config
            services.AddConsulConfig(Configuration);
            services.AddHealthChecks();

            // Couchbase
            AddCouchbase(services);

            // Mappers
            services.AddAutoMapper(typeof(MappingProfile));

            // Services
            services.AddScoped<IAdvertisementService, AdvertisementService>();

            // Repositories
            services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Hermes Advertisement API", Version = "v1"});
            });
        }

        private void RegisterJwtOptions(IServiceCollection services)
        {
            var jwtConfig = new JwtOptions();
            Configuration.Bind("JwtAuth", jwtConfig);
            services.AddSingleton(jwtConfig);
        }


        private void AddCouchbase(IServiceCollection services)
        {
            var couchbaseOptions = new CouchbaseOptions();
            Configuration.Bind("Couchbase", couchbaseOptions);

            services.AddCouchbase(client =>
                {
                    var ipList = couchbaseOptions.Servers.Select(ip => new Uri(ip)).ToList();
                    client.Servers = ipList;
                    client.UseSsl = false;
                    client.Username = couchbaseOptions.Username;
                    client.Password = couchbaseOptions.Password;
                    client.UseConnectionPooling = true;
                    client.ConnectionPool = new ConnectionPoolDefinition
                    {
                        SendTimeout = 120000,
                        MaxSize = 20,
                        MinSize = 20
                    };
                    client.OperationLifespan = 90000;
                })
                .AddCouchbaseBucket<IAdvertisementBucketProvider>("advertisements");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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