using System;
using Hermes.API.Catalog.Domain.Data;
using Hermes.API.Catalog.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Hermes.API.Catalog.Domain.Repositories
{
    public class CategoryRepository : EfCoreBaseRepository<Category, long, HermesDbContext>, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration,
            serviceProvider)
        {
        }
    }
}