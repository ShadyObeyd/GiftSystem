using GiftSystem.Models.DomainModels;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories.Contracts
{
    public interface ITransactionsRepository
    {
        Task CreateTransaction(Transaction transaction);
    }
}