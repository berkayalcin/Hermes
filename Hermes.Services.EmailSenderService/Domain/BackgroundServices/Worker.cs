using System.Threading;
using System.Threading.Tasks;
using Hermes.Services.EmailSenderService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hermes.Services.EmailSenderService.Domain.BackgroundServices
{
    public class Worker : BackgroundService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _emailSenderService.Send();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}