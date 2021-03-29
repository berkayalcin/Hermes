using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Hermes.Services.EmailSenderService.Domain.Entities;
using Hermes.Services.EmailSenderService.Domain.Enums;
using Hermes.Services.EmailSenderService.Domain.Models;
using Hermes.Services.EmailSenderService.Domain.Repositories;
using Newtonsoft.Json;

namespace Hermes.Services.EmailSenderService.Domain.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IEmailOutboxItemRepository _emailOutboxItemRepository;
        private readonly SmtpClient _smtpClient;
        private readonly SmtpOptions _smtpOptions;

        public EmailSenderService(IEmailOutboxItemRepository emailOutboxItemRepository,
            SmtpOptions smtpOptions, SmtpClient smtpClient)
        {
            _emailOutboxItemRepository = emailOutboxItemRepository;
            _smtpOptions = smtpOptions;
            _smtpClient = smtpClient;
        }

        public async Task Send()
        {
            var emailOutboxItems =
                await _emailOutboxItemRepository
                    .GetAll(e => e.StatusId == (int) EmailStatuses.Created);

            foreach (var emailOutboxItem in emailOutboxItems)
            {
                await SendEmail(emailOutboxItem);
            }
        }

        private async Task SendEmail(EmailOutboxItem item)
        {
            try
            {
                item.StatusId = (int) EmailStatuses.Locked;
                _emailOutboxItemRepository.Update(item);

                var emailOutboxItemModel = JsonConvert.DeserializeObject<EmailOutboxItemModel>(item.Body);

                var mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    Subject = emailOutboxItemModel.Subject,
                    From = _smtpOptions.Sender,
                    Sender = _smtpOptions.Sender,
                    Priority = MailPriority.High,
                    Body = emailOutboxItemModel.Body,
                    ReplyToList = {_smtpOptions.Sender}
                };

                var bcc = emailOutboxItemModel.Bcc.Select(m => new MailAddress(m)).ToList();
                var cc = emailOutboxItemModel.Cc.Select(m => new MailAddress(m)).ToList();
                var to = emailOutboxItemModel.To.Select(m => new MailAddress(m)).ToList();

                foreach (var mailAddress in bcc)
                {
                    mailMessage.Bcc.Add(mailAddress);
                }

                foreach (var mailAddress in cc)
                {
                    mailMessage.CC.Add(mailAddress);
                }

                foreach (var mailAddress in to)
                {
                    mailMessage.To.Add(mailAddress);
                }

                _smtpClient.Send(mailMessage);

                item.StatusId = (int) EmailStatuses.Sent;
                item.SentAt = DateTime.UtcNow;
                _emailOutboxItemRepository.Update(item);
            }
            catch (Exception e)
            {
                item.StatusId = (int) EmailStatuses.HasError;
                item.Error = e.ToString();
                _emailOutboxItemRepository.Update(item);
            }
        }
    }
}