using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories.Contracts
{
    public interface IUsersRepository
    {
        Task<IEnumerable<GiftSystemUser>> GetAllUsers();

        Task<GiftSystemUser> GetUserById(string id);

        Task<GiftSystemUser> GetUserWithTransactionsAndUsersById(string id);

        Task UpdateUser(GiftSystemUser user);

        Task<IEnumerable<GiftSystemUser>> GetAllUsersWithTransactions();

        Task<IdentityResult> CreateUser(GiftSystemUser user, string passowrd);

        Task SignInUser(GiftSystemUser user);

        Task LoginUser(string email, string passowrd);

        Task LogoutUser();
    }
}