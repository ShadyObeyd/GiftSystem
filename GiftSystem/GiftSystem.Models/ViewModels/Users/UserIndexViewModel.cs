using GiftSystem.Models.ViewModels.Transactions;
using System.Collections.Generic;

namespace GiftSystem.Models.ViewModels.Users
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }

        public int Credits { get; set; }

        public IEnumerable<DashboardSentTransactionsViewModel> SentTransactions { get; set; }

        public IEnumerable<DashboardReceivedTransactionsViewModel> ReceivedTransactions { get; set; }
    }
}