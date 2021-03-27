using System;
using Hermes.Services.EmailSenderService.Domain.Data;
using Hermes.Services.EmailSenderService.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Hermes.Services.EmailSenderService.Domain.Repositories
{
    public interface IEmailOutboxItemRepository : IRepository<EmailOutboxItem, Guid>
    {
    }

    public class EmailOutboxItemRepository : EfCoreBaseRepository<EmailOutboxItem, Guid, HermesDbContext>,
        IEmailOutboxItemRepository
    {
        public EmailOutboxItemRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(
            configuration, serviceProvider)
        {
        }
    }
}