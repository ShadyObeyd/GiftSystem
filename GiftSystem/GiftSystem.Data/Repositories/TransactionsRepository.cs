using GiftSystem.App.Areas.Identity.Data;
using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly GiftSystemContext db;

        public TransactionsRepository(GiftSystemContext db)
        {
            this.db = db;
        }

        public async Task CreateTransaction(Transaction transaction)
        {
            await this.db.Transactions.AddAsync(transaction);
            await this.db.SaveChangesAsync();
        }
    }
}