using System;

namespace Hermes.API.Catalog.Domain.Constants
{
    public class ConfigConstants
    {
        public const string MicrosoftLogsSourceName = "Microsoft";
        public const string ElasticSearchUrl = "ElasticSearchOptions:HostUrls";
        public const string ElasticSearchPassword = "ElasticSearchOptions:Password";
        public const string ElasticSearchUsername = "ElasticSearchOptions:Username";
        public const string SerilogApplicationPropertyName = "ApplicationName";
        public const string ApplicationName = "Hermes-Catalog-API";
        public const string SerilogEnvironmentPropertyName = "Environment";

        public static readonly string ElasticSearchLogsIndexFormat =
            $"hermes-catalog-api-{DateTime.UtcNow:yyyy-MM-dd}";
    }
}