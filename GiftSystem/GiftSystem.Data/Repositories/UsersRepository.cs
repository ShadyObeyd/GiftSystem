using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<GiftSystemUser> userManager;

        public UsersRepository(UserManager<GiftSystemUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<GiftSystemUser> GetUserById(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            return user;
        }
    }
}