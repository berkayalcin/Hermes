using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Services.AdvertisementBucketProvider;

namespace Hermes.API.Advertisement.Domain.Repositories.Advertisement
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly IBucket _bucket;

        public AdvertisementRepository(IAdvertisementBucketProvider advertisementBucketProvider)
        {
            _bucket = advertisementBucketProvider.GetBucket();
        }

        public async Task<Entities.Advertisement> Create(Entities.Advertisement advertisement)
        {
            var result = await Upsert(advertisement);
            EnsureSuccess(advertisement, result, ExceptionMessages.AdvertisementCouldNotBeInserted);
            return result.Content;
        }

        public async Task<Entities.Advertisement> Update(Entities.Advertisement advertisement)
        {
            var result = await Upsert(advertisement);
            EnsureSuccess(advertisement, result, ExceptionMessages.AdvertisementCouldNotBeUpdated);
            return result.Content;
        }

        public async Task Delete(Guid id)
        {
            var operationResult = await _bucket.RemoveAsync(id.ToString());
            if (!operationResult.Success)
            {
                var exceptionMessage = string.Format(ExceptionMessages.AdvertisementCouldNotBeDeletedWithId, id);
                throw new Exception(exceptionMessage, operationResult.Exception);
            }
        }

        public async Task<Entities.Advertisement> Get(Guid id)
        {
            var operationResult = await _bucket.GetAsync<Entities.Advertisement>(id.ToString());
            return operationResult.Success ? operationResult.Value : null;
        }

        public async Task<List<Entities.Advertisement>> GetAll()
        {
            var operationResult = await _bucket.QueryAsync<Entities.Advertisement>("SELECT advertisements.* FROM advertisements");
            return operationResult.Success ? operationResult.Rows : null;
        }

        private async Task<IDocumentResult<Entities.Advertisement>> Upsert(Entities.Advertisement advertisement)
        {
            var document = BuildAdvertisementDocument(advertisement);
            return await _bucket.UpsertAsync(document);
        }

        private static void EnsureSuccess(Entities.Advertisement advertisement, IResult result,
            string exceptionMessage)
        {
            if (result.Success) return;
            var errorMessage = string.Format(exceptionMessage, advertisement.Id);
            throw new Exception(errorMessage, result.Exception);
        }

        private Document<Entities.Advertisement> BuildAdvertisementDocument(
            Entities.Advertisement advertisement)
        {
            var guid = advertisement.Id != Guid.Empty ? advertisement.Id : Guid.NewGuid();
            advertisement.Id = guid;
            var document = new Document<Entities.Advertisement>
            {
                Id = guid.ToString(),
                Content = advertisement,
                Expiry = 0
            };

            return document;
        }
    }
}