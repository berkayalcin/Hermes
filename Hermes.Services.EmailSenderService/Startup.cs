using System;
using Hermes.Services.EmailSenderService.Domain.BackgroundServices;
using Hermes.Services.EmailSenderService.Domain.Constants;
using Hermes.Services.EmailSenderService.Domain.Data;
using Hermes.Services.EmailSenderService.Domain.Models;
using Hermes.Services.EmailSenderService.Domain.Repositories;
using Hermes.Services.EmailSenderService.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Services.EmailSenderService
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
            services.AddHealthChecks();

            // Hosted Services
            services.AddHostedService<Worker>();

            // Repositories
            services.AddScoped<IEmailOutboxItemRepository, EmailOutboxItemRepository>();

            //Services
            services.AddScoped<IEmailSenderService, Domain.Services.EmailSenderService>();

            // Data
            var connectionString = Configuration.GetValue<string>("HermesConnectionString");
            services
                .AddDbContext<HermesDbContext>(options => { options.UseSqlServer(connectionString); },
                    ServiceLifetime.Transient);

            // Email
            var smtpOptions = new SmtpOptions
            {
                Server = Environment.GetEnvironmentVariable(ConfigConstants.EmailServer),
                EmailAddress = Environment.GetEnvironmentVariable(ConfigConstants.EmailAddress),
                Password = Environment.GetEnvironmentVariable(ConfigConstants.EmailPassword),
                Port = 543
            };
            services.AddSingleton(smtpOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/healthcheck");
        }
    }
}