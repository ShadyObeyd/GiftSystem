using GiftSystem.App.Areas.Identity.Data;
using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly GiftSystemContext db;

        public UsersRepository(GiftSystemContext db)
        {
            this.db = db;
        }

        public IEnumerable<GiftSystemUser> GetAllUsers()
        {
            var users = this.db.Users.ToArray();

            return users;
        }

        public async Task<GiftSystemUser> GetUserById(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<GiftSystemUser> GetUserWithTransactionsAndUsersById(string id)
        {
            var user = await this.db.Users.Include(u => u.SentTransactions)
                                          .ThenInclude(st => st.Receiver)
                                          .Include(u => u.ReceivedTransactions)
                                          .ThenInclude(rt => rt.Sender)
                                          .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task UpdateUser(GiftSystemUser user)
        {
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }
    }
}