using System.Threading.Tasks;

namespace Hermes.Services.EmailSenderService.Domain.Services
{
    public interface IEmailSenderService
    {
        Task Send();
    }
}