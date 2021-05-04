using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Services.AdvertisementApplication
{
    public interface IAdvertisementApplicationService
    {
        Task<List<AdvertisementApplicationDto>> GetAll(
            SearchAdvertisementApplicationRequest searchAdvertisementApplicationRequest);

        Task<AdvertisementApplicationDto> GetById(long applicationId);
        Task<AdvertisementApplicationDto> Apply(AdvertisementApplicationDto advertisementApplicationDto);
        Task Approve(long applicationId);
        Task Reject(long applicationId);

        Task<AdvertisementApplicationDto> GetByAdvertisementIdAndApplicantId(Guid advertisementId,
            long applicantId);
    }
}