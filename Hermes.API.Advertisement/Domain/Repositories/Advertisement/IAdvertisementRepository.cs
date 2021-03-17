using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.API.Advertisement.Domain.Repositories.Advertisement
{
    public interface IAdvertisementRepository
    {
        Task<Entities.Advertisement> Create(Entities.Advertisement advertisement);
        Task<Entities.Advertisement> Update(Entities.Advertisement advertisement);
        Task Delete(Guid id);
        Task<Entities.Advertisement> Get(Guid id);
        Task<List<Entities.Advertisement>> GetAll();
    }
}