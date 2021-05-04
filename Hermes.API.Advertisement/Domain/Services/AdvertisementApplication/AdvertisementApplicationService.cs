using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Entities;
using Hermes.API.Advertisement.Domain.Enums;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Proxies;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Repositories.AdvertisementApplication;
using Hermes.API.Advertisement.Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hermes.API.Advertisement.Domain.Services.AdvertisementApplication
{
    public class AdvertisementApplicationService : IAdvertisementApplicationService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IAdvertisementApplicationRepository _advertisementApplicationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly IUserApiProxy _userApiProxy;

        public AdvertisementApplicationService(IAdvertisementRepository advertisementRepository,
            IAdvertisementApplicationRepository advertisementApplicationRepository,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserApiProxy userApiProxy)
        {
            _advertisementRepository = advertisementRepository;
            _advertisementApplicationRepository = advertisementApplicationRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userApiProxy = userApiProxy;
            _session = httpContextAccessor.HttpContext!.Session;
        }


        public async Task<List<AdvertisementApplicationDto>> GetAll(
            SearchAdvertisementApplicationRequest searchAdvertisementApplicationRequest)
        {
            var queryable = _advertisementApplicationRepository
                .GetQueryable()
                .Where(q => q.AdvertisementId == searchAdvertisementApplicationRequest.AdvertisementId);

            if (searchAdvertisementApplicationRequest.ApplicationStatusId.HasValue)
            {
                queryable = queryable.Where(q =>
                    q.StatusId == searchAdvertisementApplicationRequest.ApplicationStatusId.Value);
            }

            var advertisementApplications = await queryable.ToListAsync();
            var advertisementApplicationDtos =
                _mapper.Map<List<AdvertisementApplicationDto>>(advertisementApplications);
            foreach (var advertisementApplicationDto in advertisementApplicationDtos)
            {
                advertisementApplicationDto.Applicant =
                    await _userApiProxy.GetUser(advertisementApplicationDto.ApplicantId);
            }

            return advertisementApplicationDtos;
        }

        public async Task<AdvertisementApplicationDto> GetById(long applicationId)
        {
            var advertisementApplication = await _advertisementApplicationRepository
                .Get(a => a.Id == applicationId);
            if (advertisementApplication == null)
                return null;
            var advertisementApplicationDto = _mapper.Map<AdvertisementApplicationDto>(advertisementApplication);
            advertisementApplicationDto.Applicant = await _userApiProxy.GetUser(advertisementApplication.ApplicantId);

            return advertisementApplicationDto;
        }

        public async Task<AdvertisementApplicationDto> GetByAdvertisementIdAndApplicantId(Guid advertisementId,
            long applicantId)
        {
            var advertisementApplication = await _advertisementApplicationRepository
                .Get(a =>
                    a.AdvertisementId == advertisementId &&
                    a.ApplicantId == applicantId);
            if (advertisementApplication == null)
                return null;

            var advertisementApplicationDto = _mapper.Map<AdvertisementApplicationDto>(advertisementApplication);
            advertisementApplicationDto.Applicant =
                await _userApiProxy.GetUser(advertisementApplicationDto.ApplicantId);

            return advertisementApplicationDto;
        }

        public async Task<AdvertisementApplicationDto> Apply(AdvertisementApplicationDto advertisementApplicationDto)
        {
            var advertisement = await _advertisementRepository.Get(advertisementApplicationDto.AdvertisementId);
            if (advertisement == null || advertisement.StatusId != (int) AdvertisementStatuses.Created)
            {
                throw new InvalidOperationException("The Advertisement is not available to apply.");
            }

            if (advertisement.EstimatedBorrowDays < advertisementApplicationDto.EstimatedBorrowDays)
            {
                throw new InvalidOperationException("The Barrow Day Cannot Be Greater Than Advertisement's Barrow Day");
            }

            var advertisementApplication = _mapper.Map<Entities.AdvertisementApplication>(advertisementApplicationDto);
            advertisementApplication.IsDeleted = false;
            advertisementApplication.CreatedAt = DateTime.UtcNow;
            advertisementApplication.StatusId = (int) AdvertisementApplicationStatuses.Created;
            await _advertisementApplicationRepository.Insert(advertisementApplication);

            advertisement.StatusId = (int) AdvertisementStatuses.WaitingLenderApproval;
            await _advertisementRepository.Update(advertisement);

            return _mapper.Map<AdvertisementApplicationDto>(advertisementApplication);
        }

        public async Task LenderApprove(long applicationId)
        {
            var advertisementApplication = await _advertisementApplicationRepository.Get(a => a.Id == applicationId);
            var advertisement = await _advertisementRepository.Get(advertisementApplication.AdvertisementId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            advertisementApplication.StatusId = (int) AdvertisementApplicationStatuses.Approved;
            _advertisementApplicationRepository.Update(advertisementApplication);

            advertisement.StatusId = (int) AdvertisementStatuses.WaitingBorrowerApproval;
            await _advertisementRepository.Update(advertisement);

            scope.Complete();
        }

        public async Task BorrowerApprove(long applicationId)
        {
            var advertisementApplication = await _advertisementApplicationRepository.Get(a => a.Id == applicationId);
            var advertisement = await _advertisementRepository.Get(advertisementApplication.AdvertisementId);
            var advertisementApplications = await _advertisementApplicationRepository
                .GetAll(a => a.Id != applicationId && a.AdvertisementId == advertisement.Id);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            advertisementApplication.StatusId = (int) AdvertisementApplicationStatuses.Delivered;
            _advertisementApplicationRepository.Update(advertisementApplication);

            foreach (var application in advertisementApplications)
            {
                application.StatusId = (int) AdvertisementApplicationStatuses.Rejected;
                _advertisementApplicationRepository.Update(application);
            }

            advertisement.StatusId = (int) AdvertisementStatuses.Closed;
            advertisement.EstimatedBorrowDate = DateTime.UtcNow.AddDays(advertisementApplication.EstimatedBorrowDays);

            var borrowerUserDto = await _userApiProxy.GetUser(advertisementApplication.ApplicantId);
            var borrowerUser = _mapper.Map<User>(borrowerUserDto);
            advertisement.Borrower = borrowerUser;


            await _advertisementRepository.Update(advertisement);

            scope.Complete();
        }

        public async Task LenderReject(long applicationId)
        {
            var advertisementApplication = await _advertisementApplicationRepository.Get(a => a.Id == applicationId);
            var advertisement = await _advertisementRepository.Get(advertisementApplication.AdvertisementId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            advertisementApplication.StatusId = (int) AdvertisementApplicationStatuses.Rejected;
            _advertisementApplicationRepository.Update(advertisementApplication);

            advertisement.StatusId = (int) AdvertisementStatuses.Created;
            await _advertisementRepository.Update(advertisement);

            scope.Complete();
        }

        public async Task BorrowerReject(long applicationId)
        {
            var advertisementApplication = await _advertisementApplicationRepository.Get(a => a.Id == applicationId);
            var advertisement = await _advertisementRepository.Get(advertisementApplication.AdvertisementId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            advertisementApplication.StatusId = (int) AdvertisementApplicationStatuses.Rejected;
            _advertisementApplicationRepository.Update(advertisementApplication);

            advertisement.StatusId = (int) AdvertisementStatuses.Created;
            await _advertisementRepository.Update(advertisement);

            scope.Complete();
        }
    }
}