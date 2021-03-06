using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Services.ElasticSearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hermes.API.Advertisement.Domain.Services.AdvertisementSeeder
{
    public interface IAdvertisementSeederService
    {
        Task DoWork();
    }

    public class AdvertisementSeederService : BackgroundService, IAdvertisementSeederService
    {
        private readonly ILogger<AdvertisementSeederService> _logger;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementSeederService(ILogger<AdvertisementSeederService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            using var scope = serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            _elasticSearchService = serviceProvider.GetRequiredService<IElasticSearchService>();
            _advertisementRepository = serviceProvider.GetRequiredService<IAdvertisementRepository>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(150000, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork();

                    await Task.Delay(600000, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    await Task.Delay(45000, stoppingToken);
                }
            }
        }

        public async Task DoWork()
        {
            await _elasticSearchService.Purge<Entities.Advertisement>(
                ElasticSearchConstants.AdvertisementsIndex);
            _logger.LogInformation($"Seeder Service Has Started At {DateTime.UtcNow}");
            var advertisements = await _advertisementRepository.GetAll();
            if (advertisements != null && advertisements.Any())
            {
                await SeedAdvertisementsToElasticSearch(advertisements);
            }
        }

        private async Task SeedAdvertisementsToElasticSearch(IEnumerable<Entities.Advertisement> advertisements)
        {
            await _elasticSearchService.BulkAddOrUpdate(advertisements, ElasticSearchConstants.AdvertisementsIndex);
        }
    }
}