using GiftSystem.Models.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories.Contracts
{
    public interface ITransactionsRepository
    {
        Task CreateTransaction(Transaction transaction);

        Task<IEnumerable<Transaction>> GetAllTransactionsWithSendersAndReceievers();

        Task<Transaction> GetTransactionWithSenderAndReceiverById(string id);
    }
}