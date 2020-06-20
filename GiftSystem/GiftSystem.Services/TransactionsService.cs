using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using GiftSystem.Models.InputModels.Transactions;
using GiftSystem.Models.InputModels.Users;
using GiftSystem.Models.ViewModels.Transactions;
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
        private const string TransactionNotFoundMessage = "Transaction not found!";
        private const string TransactionFoundMessage = "Transaction was found.";
        private const string UserNotFoundMessage = "User not found!";

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

        public async Task<ResultData<DetailsTransactionViewModel>> CreateTransactionDetailsViewModel(string transactionId, string userId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return new ResultData<DetailsTransactionViewModel>(TransactionNotFoundMessage, false, null);
            }

            if (string.IsNullOrEmpty(userId))
            {
                return new ResultData<DetailsTransactionViewModel>(UserNotFoundMessage, false, null);
            }

            var transaction = await this.transactionsRepository.GetTransactionWithSenderAndReceiverById(transactionId);

            if (transaction == null)
            {
                return new ResultData<DetailsTransactionViewModel>(TransactionNotFoundMessage, false, null);
            }

            var viewModel = new DetailsTransactionViewModel
            {
                Credits = transaction.Credits,
                ReceiverUsername = transaction.Receiver.UserName,
                SenderUsername = transaction.Sender.UserName,
                Wish = transaction.Wish
            };

            string ownName = "You";

            if (userId == transaction.SenderId)
            {
                viewModel.SenderUsername = ownName;
            }

            if (userId == transaction.ReceiverId)
            {
                viewModel.ReceiverUsername = ownName;
            }

            return new ResultData<DetailsTransactionViewModel>(TransactionFoundMessage, true, viewModel);
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