using System;

namespace Hermes.Services.EmailSenderService.Domain.Constants
{
    public class ConfigConstants
    {
        public const string EmailServer = "EmailServer";
        public const string EmailAddress = "EmailAddress";
        public const string EmailPassword = "EmailPassword";
        public const string MicrosoftLogsSourceName = "Microsoft";
        public const string ElasticSearchUrl = "ElasticSearchOptions:HostUrls";
        public const string ElasticSearchPassword = "ElasticSearchOptions:Password";
        public const string ElasticSearchUsername = "ElasticSearchOptions:Username";
        public const string SerilogApplicationPropertyName = "ApplicationName";
        public const string ApplicationName = "Hermes-Email-Sender-Service";
        public const string SerilogEnvironmentPropertyName = "Environment";

        public static readonly string ElasticSearchLogsIndexFormat =
            $"hermes-email-sender-service-{DateTime.UtcNow:yyyy-MM-dd}";
    }
}