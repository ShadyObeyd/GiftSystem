using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<GiftSystemUser> GetAllUsers()
        {
            var users = this.userManager.Users.ToArray();

            return users;
        }

        public async Task<GiftSystemUser> GetUserById(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            return user;
        }

        public async Task UpdateUser(GiftSystemUser user)
        {
            await this.userManager.UpdateAsync(user);
        }
    }
}