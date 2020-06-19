using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using GiftSystem.Models.InputModels.Transactions;
using GiftSystem.Models.InputModels.Users;
using GiftSystem.Services.Results;
using System.Linq;
using System.Threading.Tasks;

namespace GiftSystem.Services
{
    public class TransactionsService
    {
        private const string MissingSenderOrReceiverMessage = "There must be a {0}!";
        private const string CreditsLessThanZeroMessage = "Credits cannot be a negative ammount!";
        private const string TransactionCreatedMessage = "Created transaction.";

        private readonly ITransactionsRepository transactionsRepository;
        private readonly IUsersRepository usersRepository;

        public TransactionsService(ITransactionsRepository transactionsRepository, IUsersRepository usersRepository)
        {
            this.transactionsRepository = transactionsRepository;
            this.usersRepository = usersRepository;
        }

        public CreateTransactionInputModel CreateTransactionInputModel(string userId)
        {
            var users = this.usersRepository.GetAllUsers().Where(u => u.Id != userId);

            var model = new CreateTransactionInputModel
            {
                SenderId = userId,
                Users = users.Select(u => new CreateTransactionUserInputModel
                {
                    Id = u.Id,
                    Username = u.Email
                })
            };

            return model;
        }

        public async Task<ResultData<Transaction>> CreateTransaction(string senderId, string receiverId, string wish, int credits)
        {
            if (string.IsNullOrEmpty(senderId))
            {
                return new ResultData<Transaction>(string.Format(MissingSenderOrReceiverMessage, "sender"), false, null);
            }

            if (string.IsNullOrEmpty(receiverId))
            {
                return new ResultData<Transaction>(string.Format(MissingSenderOrReceiverMessage, "receiver"), false, null);
            }

            if (credits < 0)
            {
                return new ResultData<Transaction>(CreditsLessThanZeroMessage, false, null);
            }

            await AdjustSenderAndReceiverCredits(senderId, receiverId, credits);

            var transaction = new Transaction
            {
                Credits = credits,
                ReceiverId = receiverId,
                SenderId = senderId,
                Wish = wish
            };

            await this.transactionsRepository.CreateTransaction(transaction);

            return new ResultData<Transaction>(TransactionCreatedMessage, true, transaction);
        }

        private async Task AdjustSenderAndReceiverCredits(string senderId, string receiverId, int credits)
        {
            var sender = await this.usersRepository.GetUserById(senderId);

            sender.Credits -= credits;

            await usersRepository.UpdateUser(sender);

            var receiver = await this.usersRepository.GetUserById(receiverId);

            receiver.Credits += credits;

            await usersRepository.UpdateUser(receiver);
        }
    }
}