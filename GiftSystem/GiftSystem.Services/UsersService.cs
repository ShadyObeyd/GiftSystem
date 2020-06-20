using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.ViewModels.Users;
using System.Threading.Tasks;
using GiftSystem.Services.Results;
using GiftSystem.Models.DomainModels;
using System.Linq;
using GiftSystem.Models.ViewModels.Transactions;

namespace GiftSystem.Services
{
    public class UsersService
    {
        private const string UserNotFoundMessage = "User not found!";
        private const string UserWasFoundMessage = "User found!";
        private const string AllUsersViewModelCreatedMessage = "All users view model created.";

        private readonly IUsersRepository usersRepository;
        private readonly ITransactionsRepository transactionsRepository;

        public UsersService(IUsersRepository usersRepository, ITransactionsRepository transactionsRepository)
        {
            this.usersRepository = usersRepository;
            this.transactionsRepository = transactionsRepository;
        }

        public async Task<ResultData<UserIndexViewModel>> CreateIndexViewModel(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new ResultData<UserIndexViewModel>(UserNotFoundMessage, false, null);
            }

            var user = await this.usersRepository.GetUserWithTransactionsAndUsersById(userId);

            if (user == null)
            {
                return new ResultData<UserIndexViewModel>(UserNotFoundMessage, false, null);
            }

            var transactions = await this.transactionsRepository.GetAllTransactionsWithSendersAndReceievers();

            var viewModel = new UserIndexViewModel
            {
                Credits = user.Credits,
                Id = user.Id,
                SentTransactions = user.SentTransactions.Select(st => new DashboardSentTransactionsViewModel
                {
                    Id = st.Id,
                    Credits = st.Credits,
                    ReceiverUsername = st.Receiver.UserName
                }),
                ReceivedTransactions = user.ReceivedTransactions.Select(rt => new DashboardReceivedTransactionsViewModel
                {
                    Id = rt.Id,
                    Credits = rt.Credits,
                    SenderUsername = rt.Sender.UserName,
                }),
                AllTransactions = transactions.Select(t => new AllTransactionsViewModel
                {
                    Id = t.Id,
                    SenderUsername = t.Sender.Id == userId ? "You" : t.Sender.UserName,
                    ReceiverUsername = t.Receiver.Id == userId ? "you" : t.Receiver.UserName,
                    Credits = t.Credits
                })
            };
            return new ResultData<UserIndexViewModel>(UserWasFoundMessage, true, viewModel);
        }

        public async Task<ResultData<GiftSystemUser>> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new ResultData<GiftSystemUser>(UserNotFoundMessage, false, null);
            }

            var user = await this.usersRepository.GetUserById(userId);

            return new ResultData<GiftSystemUser>(UserWasFoundMessage, true, user);
        }

        public async Task<ResultData<AllUsersViewModel>> CreateAllUsersViewModel()
        {
            var users = await this.usersRepository.GetAllUsersWithTransactions();

            var viewModel = new AllUsersViewModel
            {
                Users = users.Select(u => new AllUsersUserViewModel
                {
                    Username = u.UserName,
                    Credits = u.Credits,
                    SentTransactionsCount = u.SentTransactions.Count(),
                    ReceivedTransactionsCount = u.ReceivedTransactions.Count()
                })
            };

            return new ResultData<AllUsersViewModel>(AllUsersViewModelCreatedMessage, true, viewModel);
        }
    }
}