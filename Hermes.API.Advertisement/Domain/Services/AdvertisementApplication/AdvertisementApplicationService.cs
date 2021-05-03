using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Enums;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Repositories.AdvertisementApplication;
using Hermes.API.Advertisement.Domain.Requests;
using Microsoft.AspNetCore.Http;

namespace Hermes.API.Advertisement.Domain.Services.AdvertisementApplication
{
    public class AdvertisementApplicationService : IAdvertisementApplicationService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IAdvertisementApplicationRepository _advertisementApplicationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public AdvertisementApplicationService(IAdvertisementRepository advertisementRepository,
            IAdvertisementApplicationRepository advertisementApplicationRepository,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _advertisementApplicationRepository = advertisementApplicationRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _session = httpContextAccessor.HttpContext!.Session;
        }

        private bool IsSameUserOrAdmin(long dataOwnerId)
        {
            var id = long.Parse(_session.GetString(SessionClaimTypes.Id));
            var role = _session.GetString(SessionClaimTypes.Role);
            if (role.Equals(UserRoles.Administrator))
                return true;
            return id == dataOwnerId;
        }

        public async Task<List<AdvertisementApplicationDto>> GetAll(
            SearchAdvertisementApplicationRequest searchAdvertisementApplicationRequest)
        {
            var advertisement =
                await _advertisementRepository.Get(searchAdvertisementApplicationRequest.AdvertisementId);
            if (!IsSameUserOrAdmin(advertisement.UserId))
            {
                throw new UnauthorizedAccessException();
            }


            throw new NotImplementedException();
        }

        public async Task<AdvertisementApplicationDto> GetById(long applicationId)
        {
            var advertisementApplication = await _advertisementApplicationRepository
                .Get(a => a.Id == applicationId);
            return advertisementApplication == null
                ? null
                : _mapper.Map<AdvertisementApplicationDto>(advertisementApplication);
        }

        public async Task<AdvertisementApplicationDto> GetByAdvertisementIdAndApplicantId(Guid advertisementId,
            long applicantId)
        {
            var advertisementApplication = await _advertisementApplicationRepository
                .Get(a =>
                    a.AdvertisementId == advertisementId &&
                    a.ApplicantId == applicantId);
            return advertisementApplication == null
                ? null
                : _mapper.Map<AdvertisementApplicationDto>(advertisementApplication);
        }

        public async Task<AdvertisementApplicationDto> Apply(AdvertisementApplicationDto advertisementApplicationDto)
        {
            var advertisement = await _advertisementRepository.Get(advertisementApplicationDto.AdvertisementId);
            if (advertisement == null || advertisement.StatusId != (int) AdvertisementStatuses.Created)
            {
                throw new InvalidOperationException("The Advertisement is not available to apply.");
            }

            if (advertisement.EstimatedBarrowDays < advertisementApplicationDto.EstimatedBarrowDays)
            {
                throw new InvalidOperationException("The Barrow Day Cannot Be Greater Than Advertisement's Barrow Day");
            }

            var advertisementApplication = _mapper.Map<Entities.AdvertisementApplication>(advertisementApplicationDto);
            advertisementApplication.IsDeleted = false;
            advertisementApplication.CreatedAt = DateTime.UtcNow;
            await _advertisementApplicationRepository.Insert(advertisementApplication);

            advertisement.StatusId = (int) AdvertisementStatuses.WaitingLenderApproval;
            await _advertisementRepository.Update(advertisement);

            return _mapper.Map<AdvertisementApplicationDto>(advertisementApplication);
        }

        public Task Approve(Guid applicationId)
        {
            throw new NotImplementedException();
        }

        public Task Reject(Guid applicationId)
        {
            throw new NotImplementedException();
        }
    }
}