using System;

namespace Hermes.API.Notification.Domain.Constants
{
    public class ConfigConstants
    {
        public const string MicrosoftLogsSourceName = "Microsoft";
        public const string ElasticSearchUrl = "ElasticSearchOptions:HostUrls";
        public const string ElasticSearchPassword = "ElasticSearchOptions:Password";
        public const string ElasticSearchUsername = "ElasticSearchOptions:Username";
        public const string SerilogApplicationPropertyName = "ApplicationName";
        public const string ApplicationName = "Hermes-Notification-API";
        public const string SerilogEnvironmentPropertyName = "Environment";

        public static readonly string ElasticSearchLogsIndexFormat =
            $"hermes-notification-api-{DateTime.UtcNow:yyyy-MM-dd}";
    }
}