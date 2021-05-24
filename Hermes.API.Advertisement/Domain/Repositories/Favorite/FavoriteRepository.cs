using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Services.FavoriteBucketProvider;

namespace Hermes.API.Advertisement.Domain.Repositories.Favorite
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IBucket _bucket;

        public FavoriteRepository(IFavoriteBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket();
        }

        public async Task<List<Entities.Favorite>> GetAllByUserId(long userId)
        {
            var queryRequest = new QueryRequest()
                .Statement("SELECT favorites.* FROM favorites where userId=$1")
                .AddPositionalParameter(userId);

            var operationResult = await _bucket.QueryAsync<Entities.Favorite>(queryRequest);
            return operationResult.Success ? operationResult.Rows : null;
        }

        public async Task<Entities.Favorite> GetByUserIdAndAdvertisementId(long userId, Guid advertisementId)
        {
            var queryRequest = new QueryRequest()
                .Statement("SELECT favorites.* FROM favorites where userId=$1 and advertisementId=$2")
                .AddPositionalParameter(userId)
                .AddPositionalParameter(advertisementId);

            var operationResult = await _bucket.QueryAsync<Entities.Favorite>(queryRequest);
            return operationResult.Success ? operationResult.Rows.FirstOrDefault() : null;
        }

        public async Task DeleteByUserIdAndAdvertisementId(long userId, Guid advertisementId)
        {
            var queryRequest = new QueryRequest()
                .Statement("SELECT favorites.* FROM favorites where userId=$1 and advertisementId=$2")
                .AddPositionalParameter(userId)
                .AddPositionalParameter(advertisementId);

            var operationResult = await _bucket.QueryAsync<Entities.Favorite>(queryRequest);
            if (!operationResult.Success)
            {
                var exceptionMessage = string.Format(ExceptionMessages.FavoriteCouldNotBeDeleted);
                throw new Exception(exceptionMessage, operationResult.Exception);
            }

            foreach (var favorite in operationResult.Rows)
            {
                await Delete(favorite.Id);
            }
        }


        public async Task<Entities.Favorite> Insert(Entities.Favorite favorite)
        {
            var result = await Upsert(favorite);
            EnsureSuccess(favorite, result, ExceptionMessages.FavoriteCouldNotBeInserted);
            return result.Content;
        }

        public async Task<Entities.Favorite> Update(Entities.Favorite favorite)
        {
            var result = await Upsert(favorite);
            EnsureSuccess(favorite, result, ExceptionMessages.FavoriteCouldNotBeUpdated);
            return result.Content;
        }

        public async Task Delete(Guid id)
        {
            var operationResult = await _bucket.RemoveAsync(id.ToString());
            if (!operationResult.Success)
            {
                var exceptionMessage = string.Format(ExceptionMessages.FavoriteCouldNotBeDeletedWithId, id);
                throw new Exception(exceptionMessage, operationResult.Exception);
            }
        }

        private async Task<IDocumentResult<Entities.Favorite>> Upsert(Entities.Favorite favorite)
        {
            var document = BuildFavoriteDocument(favorite);
            return await _bucket.UpsertAsync(document);
        }

        private static void EnsureSuccess(Entities.Favorite favorite, IResult result,
            string exceptionMessage)
        {
            if (result.Success) return;
            var errorMessage = string.Format(exceptionMessage, favorite.Id);
            throw new Exception(errorMessage, result.Exception);
        }

        private Document<Entities.Favorite> BuildFavoriteDocument(
            Entities.Favorite favorite)
        {
            var guid = favorite.Id != Guid.Empty ? favorite.Id : Guid.NewGuid();
            favorite.Id = guid;
            var document = new Document<Entities.Favorite>
            {
                Id = guid.ToString(),
                Content = favorite,
                Expiry = 0
            };

            return document;
        }
    }
}