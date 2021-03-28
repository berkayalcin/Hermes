namespace Hermes.Services.EmailSenderService.Domain.Models
{
    public class SmtpOptions
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }
}