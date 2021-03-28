using System.Threading.Tasks;
using Hermes.Services.EmailSenderService.Domain.Entities;
using Hermes.Services.EmailSenderService.Domain.Enums;
using Hermes.Services.EmailSenderService.Domain.Models;
using Hermes.Services.EmailSenderService.Domain.Repositories;

namespace Hermes.Services.EmailSenderService.Domain.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IEmailOutboxItemRepository _emailOutboxItemRepository;
        private readonly SmtpOptions _smtpOptions;

        public EmailSenderService(IEmailOutboxItemRepository emailOutboxItemRepository,
            SmtpOptions smtpOptions)
        {
            _emailOutboxItemRepository = emailOutboxItemRepository;
            _smtpOptions = smtpOptions;
        }

        public async Task Send()
        {
            var emailOutboxItems =
                await _emailOutboxItemRepository.GetAll(e => e.StatusId == (int) EmailStatuses.Created);
            foreach (var emailOutboxItem in emailOutboxItems)
            {
            }
        }

        private async Task SendEmail(EmailOutboxItem item)
        {
            ;
        }
    }
}