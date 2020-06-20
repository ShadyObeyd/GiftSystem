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
        private readonly IUsersRepository usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
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

            var viewModel = new UserIndexViewModel
            {
                Credits = user.Credits,
                Id = user.Id,
                SentTransactions = user.SentTransactions.Select(st => new DashboardSentTransactionsViewModel
                {
                    Id = st.Id,
                    ReceiverId = st.ReceiverId,
                    Credits = st.Credits,
                    ReceiverUsername = st.Receiver.UserName
                }),
                ReceivedTransactions = user.ReceivedTransactions.Select(rt => new DashboardReceivedTransactionsViewModel
                {
                    Id = rt.Id,
                    SenderId = rt.SenderId,
                    Credits = rt.Credits,
                    SenderUsername = rt.Sender.UserName,
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
    }
}