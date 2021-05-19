using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Services.UserReviewBucketProvider;

namespace Hermes.API.Advertisement.Domain.Repositories.UserReview
{
    public class UserReviewRepository : IUserReviewRepository
    {
        private readonly IBucket _bucket;

        public UserReviewRepository(IUserReviewBucketProvider userReviewBucketProvider)
        {
            _bucket = userReviewBucketProvider.GetBucket();
        }

        public async Task<Entities.UserReview> Create(Entities.UserReview userReview)
        {
            var result = await Upsert(userReview);
            EnsureSuccess(userReview, result, ExceptionMessages.UserReviewCouldNotBeInserted);
            return result.Content;
        }

        public async Task<Entities.UserReview> Update(Entities.UserReview userReview)
        {
            var result = await Upsert(userReview);
            EnsureSuccess(userReview, result, ExceptionMessages.UserReviewCouldNotBeUpdated);
            return result.Content;
        }

        private async Task<IDocumentResult<Entities.UserReview>> Upsert(Entities.UserReview userReview)
        {
            var document = BuildUserReviewDocument(userReview);
            return await _bucket.UpsertAsync(document);
        }

        private static void EnsureSuccess(Entities.UserReview userReview, IResult result,
            string exceptionMessage)
        {
            if (result.Success) return;
            var errorMessage = string.Format(exceptionMessage, userReview.Id);
            throw new Exception(errorMessage, result.Exception);
        }

        private Document<Entities.UserReview> BuildUserReviewDocument(
            Entities.UserReview userReview)
        {
            var guid = userReview.Id != Guid.Empty ? userReview.Id : Guid.NewGuid();
            userReview.Id = guid;
            var document = new Document<Entities.UserReview>
            {
                Id = guid.ToString(),
                Content = userReview,
                Expiry = 0
            };

            return document;
        }

        public async Task Delete(Guid id)
        {
            var operationResult = await _bucket.RemoveAsync(id.ToString());
            if (!operationResult.Success)
            {
                var exceptionMessage = string.Format(ExceptionMessages.UserReviewCouldNotBeDeletedWithId, id);
                throw new Exception(exceptionMessage, operationResult.Exception);
            }
        }

        public async Task<Entities.UserReview> Get(Guid id)
        {
            var operationResult = await _bucket.GetAsync<Entities.UserReview>(id.ToString());
            return operationResult.Success ? operationResult.Value : null;
        }

        public async Task<List<Entities.UserReview>> GetAll()
        {
            var operationResult =
                await _bucket.QueryAsync<Entities.UserReview>("SELECT 'userreviews'.* FROM 'userreviews'");
            return operationResult.Success ? operationResult.Rows : null;
        }

        public async Task<List<Entities.UserReview>> GetAllByOwnerId(long ownerId)
        {
            var queryRequest = new QueryRequest()
                .Statement("SELECT userreviews.* FROM userreviews where reviewOwnerId=$1")
                .AddPositionalParameter(ownerId);

            var operationResult = await _bucket.QueryAsync<Entities.UserReview>(queryRequest);
            return operationResult.Success ? operationResult.Rows : null;
        }

        public async Task<List<Entities.UserReview>> GetAllByReviewedUserId(long reviewedUserId)
        {
            var queryRequest = new QueryRequest()
                .Statement("SELECT userreviews.* FROM userreviews where reviewedUserId=$1")
                .AddPositionalParameter(reviewedUserId);

            var operationResult = await _bucket.QueryAsync<Entities.UserReview>(queryRequest);
            return operationResult.Success ? operationResult.Rows : null;
        }

        public async Task<List<Entities.UserReview>> GetAllByOwnerIdAndApplicationId(long ownerId, long applicationId)
        {
            var queryRequest = new QueryRequest()
                .Statement("SELECT userreviews.* FROM userreviews where reviewOwnerId=$1 and applicationId=$2")
                .AddPositionalParameter(ownerId)
                .AddPositionalParameter(applicationId);

            var operationResult = await _bucket.QueryAsync<Entities.UserReview>(queryRequest);
            return operationResult.Success ? operationResult.Rows : null;
        }
    }
}