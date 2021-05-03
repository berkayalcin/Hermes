using System;
using Hermes.API.Advertisement.Domain.Repositories.EfCore;

namespace Hermes.API.Advertisement.Domain.Repositories.AdvertisementApplication
{
    public interface IAdvertisementApplicationRepository : IRepository<Entities.AdvertisementApplication, Guid>
    {
    }
}