using System.Collections.Generic;

namespace GiftSystem.Models.ViewModels.Users
{
    public class AllUsersViewModel
    {
        public IEnumerable<AllUsersUserViewModel> Users { get; set; }
    }
}