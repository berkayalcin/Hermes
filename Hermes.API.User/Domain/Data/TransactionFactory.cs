using System.Transactions;

namespace Hermes.API.User.Domain.Data
{
    public class TransactionFactory
    {
        public static TransactionScope Create()
        {
            return new(TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}