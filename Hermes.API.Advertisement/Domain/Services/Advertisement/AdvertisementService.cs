using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Entities;
using Hermes.API.Advertisement.Domain.Enums;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Proxies;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Requests;
using Hermes.API.Advertisement.Domain.Responses;
using Hermes.API.Advertisement.Domain.Services.ElasticSearch;
using Nest;

namespace Hermes.API.Advertisement.Domain.Services.Advertisement
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly ICategoryApiProxy _categoryApiProxy;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IMapper _mapper;
        private readonly IUserApiProxy _userApiProxy;

        public AdvertisementService(IAdvertisementRepository advertisementRepository, IMapper mapper,
            IElasticSearchService elasticSearchService, ICategoryApiProxy categoryApiProxy, IUserApiProxy userApiProxy)
        {
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
            _elasticSearchService = elasticSearchService;
            _categoryApiProxy = categoryApiProxy;
            _userApiProxy = userApiProxy;
        }

        public async Task<AdvertisementDto> Create(CreateAdvertisementRequest createAdvertisementRequest)
        {
            var advertisement = _mapper.Map<Entities.Advertisement>(createAdvertisementRequest);
            advertisement.IsDeleted = false;
            advertisement.CreatedAt = DateTime.UtcNow;
            advertisement.StatusId = (int) AdvertisementStatuses.Created;
            advertisement.Location = new Geolocation
            {
                Lat = advertisement.Latitude,
                Lon = advertisement.Longitude
            };

            advertisement.Category = await TryGetCategory(createAdvertisementRequest.CategoryId);
            advertisement.User = await TryGetUser(createAdvertisementRequest.UserId);
            advertisement = await _advertisementRepository.Create(advertisement);

            await SeedAdvertisementToElasticSearch(advertisement);
            return _mapper.Map<AdvertisementDto>(advertisement);
        }

        public async Task<AdvertisementDto> Update(UpdateAdvertisementRequest updateAdvertisementRequest)
        {
            var advertisement = await _advertisementRepository.Get(updateAdvertisementRequest.Id);
            if (advertisement == null)
            {
                throw new ArgumentNullException(nameof(advertisement));
            }

            var updatedAdvertisement = _mapper.Map<Entities.Advertisement>(updateAdvertisementRequest);
            updatedAdvertisement.Location = BuildGeolocation(advertisement);
            updatedAdvertisement.Category = await TryGetCategory(updateAdvertisementRequest.CategoryId);
            updatedAdvertisement.User = await TryGetUser(updateAdvertisementRequest.UserId);

            updatedAdvertisement = await _advertisementRepository.Update(updatedAdvertisement);
            await SeedAdvertisementToElasticSearch(updatedAdvertisement);

            return _mapper.Map<AdvertisementDto>(updatedAdvertisement);
        }


        public async Task Delete(Guid id)
        {
            var advertisement = await _advertisementRepository.Get(id);
            if (advertisement == null)
            {
                throw new ArgumentNullException(nameof(advertisement));
            }

            await _advertisementRepository.Delete(id);
        }

        public async Task<AdvertisementDto> Get(Guid id)
        {
            var advertisement = await _advertisementRepository.Get(id);
            return advertisement == null ? null : _mapper.Map<AdvertisementDto>(advertisement);
        }

        public async Task<PagedResponse<AdvertisementDto>> GetAll(SearchAdvertisementRequest searchAdvertisementRequest)
        {
            var searchResponse =
                await _elasticSearchService.Search<Entities.Advertisement>(s =>
                    s
                        .Index(ElasticSearchConstants.AdvertisementsIndex)
                        .From(searchAdvertisementRequest.SkipCount)
                        .Size(searchAdvertisementRequest.PageSize)
                        .Sort(so =>
                            so.Field(f =>
                                f
                                    .Field(p => p.CreatedAt)
                                    .Order(searchAdvertisementRequest.OrderByDesc
                                        ? SortOrder.Descending
                                        : SortOrder.Ascending)
                            )
                        )
                        .Query(q =>
                        {
                            var queryContainer = q.MultiMatch(c => c
                                                     .Type(TextQueryType.PhrasePrefix)
                                                     .Query(searchAdvertisementRequest.Query)
                                                     .Boost(1.1)
                                                     .Slop(2)
                                                     .PrefixLength(2)
                                                     .MaxExpansions(50)
                                                     .Operator(Operator.Or)
                                                     .MinimumShouldMatch(2)
                                                     .ZeroTermsQuery(ZeroTermsQuery.All)
                                                 ) &&
                                                 q.Term(c =>
                                                     c
                                                         .Boost(1.1)
                                                         .Field(f => f.CategoryId)
                                                         .Value(searchAdvertisementRequest.CategoryId)
                                                 ) &&
                                                 q.Term(c =>
                                                     c
                                                         .Boost(1.1)
                                                         .Field(f => f.StatusId)
                                                         .Value(searchAdvertisementRequest.StatusId)
                                                 ) &&
                                                 q.Range(c => c
                                                     .Boost(1.1)
                                                     .Field(p => p.EstimatedBorrowDays)
                                                     .GreaterThanOrEquals(searchAdvertisementRequest
                                                         .EstimatedBorrowDaysMin)
                                                     .LessThanOrEquals(
                                                         searchAdvertisementRequest.EstimatedBorrowDaysMax)
                                                     .Relation(RangeRelation.Within)
                                                 );

                            if (searchAdvertisementRequest.Longitude.HasValue &&
                                searchAdvertisementRequest.Latitude.HasValue)
                            {
                                return queryContainer &&
                                       q.GeoDistance(g =>
                                           g
                                               .Boost(1.1)
                                               .Field(p => p.Location)
                                               .DistanceType(GeoDistanceType.Plane)
                                               .Location(
                                                   searchAdvertisementRequest.Latitude.Value,
                                                   searchAdvertisementRequest.Longitude.Value
                                               )
                                               .Distance(new Distance(10, DistanceUnit.Kilometers))
                                       );
                            }

                            return queryContainer;
                        })
                );

            var advertisementDtos = _mapper.Map<List<AdvertisementDto>>(searchResponse.Documents.ToList());

            return new PagedResponse<AdvertisementDto>()
            {
                Items = advertisementDtos,
                TotalCount = searchResponse.Total
            };
        }

        private static Geolocation BuildGeolocation(Entities.Advertisement advertisement)
        {
            return new()
            {
                Lat = advertisement.Latitude,
                Lon = advertisement.Longitude
            };
        }

        private async Task<Category> TryGetCategory(long categoryId)
        {
            var category = await _categoryApiProxy.Get(categoryId);
            if (category == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.CategoryWithIdIsNotExists,
                    categoryId));
            }

            return _mapper.Map<Category>(category);
        }

        private async Task<User> TryGetUser(long userId)
        {
            var user = await _userApiProxy.GetUser(userId);
            if (user == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.UserWithIdIsNotExists, userId));
            }

            return _mapper.Map<User>(user);
        }

        private async Task SeedAdvertisementToElasticSearch(Entities.Advertisement advertisement)
        {
            await _elasticSearchService.CreateIndexIfNotExists<Entities.Advertisement>(ElasticSearchConstants
                .AdvertisementsIndex);
            await _elasticSearchService.AddOrUpdate(advertisement, ElasticSearchConstants.AdvertisementsIndex);
        }
    }
}