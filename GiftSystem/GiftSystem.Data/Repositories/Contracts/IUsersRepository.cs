using GiftSystem.Models.DomainModels;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories.Contracts
{
    public interface IUsersRepository
    {
        Task<GiftSystemUser> GetUserById(string id);
    }
}