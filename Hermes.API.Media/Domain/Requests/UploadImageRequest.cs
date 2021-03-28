namespace Hermes.API.Media.Domain.Requests
{
    public class UploadImageRequest
    {
        public string Base64String { get; set; }
        public string Name { get; set; }
    }
}