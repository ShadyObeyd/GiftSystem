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
        private const string UserLoggedOutMessage = "User was logged out.";
        private const string FieldIsRequiredMessage = "{0} is required!";
        private const string PassowrdsDoNotMatchMessage = "Passwords do not match!";
        private const string RegisterFailedMessage = "Register failed!";
        private const string RegisteredSuccessfullyMessage = "User reigsered.";

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

        public async Task<Result> LogoutUser()
        {
            await this.usersRepository.LogoutUser();

            return new Result(UserLoggedOutMessage, true);
        }

        public async Task<Result> RegisterUser(string email, string password, string rePassword, string phoneNumber)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new Result(string.Format(FieldIsRequiredMessage, email), false);
            }

            if (string.IsNullOrEmpty(password))
            {
                return new Result(string.Format(FieldIsRequiredMessage, password), false);
            }

            if (string.IsNullOrEmpty(rePassword))
            {
                return new Result(string.Format(FieldIsRequiredMessage, rePassword), false);
            }

            if (password != rePassword)
            {
                return new Result(PassowrdsDoNotMatchMessage, false);
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return new Result(string.Format(FieldIsRequiredMessage, phoneNumber), false);
            }

            var user = new GiftSystemUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var result = await this.usersRepository.CreateUser(user, password);

            if (!result.Succeeded)
            {
                return new Result(RegisterFailedMessage, false);
            }
            await this.usersRepository.AsignUserToRole(user);
            await this.usersRepository.SignInUser(user);

            return new Result(RegisteredSuccessfullyMessage, true);
        }

        public async Task<Result> LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return new Result(UserNotFoundMessage, false);
            }

            await this.usersRepository.LoginUser(email, password);

            return new Result(UserWasFoundMessage, true);
        }
    }
}