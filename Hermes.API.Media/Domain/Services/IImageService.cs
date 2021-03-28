using System.Threading.Tasks;
using Hermes.API.Media.Domain.Models;
using Hermes.API.Media.Domain.Requests;

namespace Hermes.API.Media.Domain.Services
{
    public interface IImageService
    {
        Task<ImageDto> Upload(UploadImageRequest uploadImageRequest);
    }
}