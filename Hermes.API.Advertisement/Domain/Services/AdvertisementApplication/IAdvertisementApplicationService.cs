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
        Task<AdvertisementApplicationDto> GetById(Guid applicationId);
        Task<AdvertisementApplicationDto> Apply(AdvertisementApplicationDto advertisementApplicationDto);
        Task Approve(Guid applicationId);
        Task Reject(Guid applicationId);

        Task<AdvertisementApplicationDto> GetByAdvertisementIdAndApplicantId(Guid advertisementId,
            long applicantId);
    }
}