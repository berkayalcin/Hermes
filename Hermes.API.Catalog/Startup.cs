using System.Linq;
using FluentValidation.AspNetCore;
using Hermes.API.Catalog.Domain.Authentication;
using Hermes.API.Catalog.Domain.Data;
using Hermes.API.Catalog.Domain.Filters;
using Hermes.API.Catalog.Domain.Mappers;
using Hermes.API.Catalog.Domain.Repositories;
using Hermes.API.Catalog.Domain.Responses;
using Hermes.API.Catalog.Domain.Services;
using Hermes.API.Catalog.Domain.Validators;
using Hermes.API.Catalog.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Hermes.API.Catalog
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
                .AddFluentValidation(o => { o.RegisterValidatorsFromAssemblyContaining<CategoryDtoValidator>(); }
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Hermes Catalog API", Version = "v1"});
            });

            // Services
            services.AddScoped<ICategoryService, CategoryService>();

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Data
            var connectionString = Configuration.GetValue<string>("HermesConnectionString");
            services
                .AddDbContext<HermesDbContext>(options => { options.UseSqlServer(connectionString); },
                    ServiceLifetime.Transient);

            // Mappers
            services.AddAutoMapper(typeof(MappingProfile));

            // Auth
            RegisterJwtOptions(services);

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