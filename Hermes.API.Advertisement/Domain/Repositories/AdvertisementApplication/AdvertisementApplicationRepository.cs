using System;
using Hermes.API.Advertisement.Domain.Data;
using Hermes.API.Advertisement.Domain.Repositories.EfCore;
using Microsoft.Extensions.Configuration;

namespace Hermes.API.Advertisement.Domain.Repositories.AdvertisementApplication
{
    class AdvertisementApplicationRepository :
        EfCoreBaseRepository<Entities.AdvertisementApplication, Guid, HermesDbContext>,
        IAdvertisementApplicationRepository
    {
        public AdvertisementApplicationRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {
        }
    }
}