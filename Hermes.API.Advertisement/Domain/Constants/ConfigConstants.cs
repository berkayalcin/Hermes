using System;

namespace Hermes.API.Advertisement.Domain.Constants
{
    public static class ConfigConstants
    {
        public const string MicrosoftLogsSourceName = "Microsoft";
        public const string ElasticSearchUrl = "ElasticSearchOptions:HostUrls";
        public const string ElasticSearchPassword = "ElasticSearchOptions:Password";
        public const string ElasticSearchUsername = "ElasticSearchOptions:Username";
        public const string SerilogApplicationPropertyName = "ApplicationName";
        public const string ApplicationName = "Hermes-Advertisement-API";
        public const string SerilogEnvironmentPropertyName = "Environment";

        public const string GatewayBaseUrl = "GatewayBaseUrl";

        public static readonly string ElasticSearchLogsIndexFormat =
            $"hermes-advertisement-api-{DateTime.UtcNow:yyyy-MM-dd}";
    }
}