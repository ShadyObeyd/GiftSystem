using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.ViewModels.Users;
using System.Threading.Tasks;
using GiftSystem.Services.Results;
using GiftSystem.Models.DomainModels;

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

            var user = await this.usersRepository.GetUserById(userId);

            if (user == null)
            {
                return new ResultData<UserIndexViewModel>(UserNotFoundMessage, false, null);
            }

            var viewModel = new UserIndexViewModel
            {
                Credits = user.Credits,
                Id = user.Id
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