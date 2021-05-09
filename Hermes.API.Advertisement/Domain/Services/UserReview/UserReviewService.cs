using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Entities;
using Hermes.API.Advertisement.Domain.Enums;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Proxies;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Repositories.AdvertisementApplication;
using Hermes.API.Advertisement.Domain.Repositories.UserReview;

namespace Hermes.API.Advertisement.Domain.Services.UserReview
{
    public class UserReviewService : IUserReviewService
    {
        private readonly IUserReviewRepository _userReviewRepository;
        private readonly IAdvertisementApplicationRepository _advertisementApplicationRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;
        private readonly IUserApiProxy _userApiProxy;

        public UserReviewService(IUserReviewRepository userReviewRepository,
            IAdvertisementApplicationRepository advertisementApplicationRepository,
            IAdvertisementRepository advertisementRepository, IMapper mapper, IUserApiProxy userApiProxy)
        {
            _userReviewRepository = userReviewRepository;
            _advertisementApplicationRepository = advertisementApplicationRepository;
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
            _userApiProxy = userApiProxy;
        }

        public async Task<UserReviewDto> Create(UserReviewDto userReviewDto)
        {
            var advertisementApplication =
                await _advertisementApplicationRepository.Get(g => g.Id == userReviewDto.ApplicationId);
            if (advertisementApplication.StatusId != (int) AdvertisementApplicationStatuses.LenderTookItemBack)
            {
                throw new InvalidOperationException("Can't create review for this application");
            }

            var advertisement = await _advertisementRepository.Get(advertisementApplication.AdvertisementId);
            var userReview = _mapper.Map<Entities.UserReview>(userReviewDto);
            userReview.Advertisement = advertisement;
            userReview.ReviewOwner = _mapper.Map<User>(await _userApiProxy.GetUser(userReview.ReviewOwnerId));
            userReview.ReviewedUser = _mapper.Map<User>(await _userApiProxy.GetUser(userReview.ReviewedUserId));
            return _mapper.Map<UserReviewDto>(await _userReviewRepository.Create(userReview));
        }

        public async Task<List<UserReviewDto>> GetAllByUserId(long userId)
        {
            var userReviews = await _userReviewRepository.GetAllByOwnerId(userId);
            return userReviews == null ? null : _mapper.Map<List<UserReviewDto>>(userReviews);
        }
    }
}