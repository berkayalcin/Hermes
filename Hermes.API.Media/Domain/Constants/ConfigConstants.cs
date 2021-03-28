using System;

namespace Hermes.API.Media.Domain.Constants
{
    public static class ConfigConstants
    {
        public const string MicrosoftLogsSourceName = "Microsoft";
        public const string ElasticSearchUrl = "ElasticSearchOptions:HostUrls";
        public const string ElasticSearchPassword = "ElasticSearchOptions:Password";
        public const string ElasticSearchUsername = "ElasticSearchOptions:Username";
        public const string SerilogApplicationPropertyName = "ApplicationName";
        public const string ApplicationName = "Hermes-Media-API";
        public const string SerilogEnvironmentPropertyName = "Environment";

        public const string AwsAccessKey = "AwsS3AccessKeyId";
        public const string AwsSecretKey = "AwsS3SecretKey";

        public static readonly string ElasticSearchLogsIndexFormat =
            $"hermes-media-api-{DateTime.UtcNow:yyyy-MM-dd}";
    }
}