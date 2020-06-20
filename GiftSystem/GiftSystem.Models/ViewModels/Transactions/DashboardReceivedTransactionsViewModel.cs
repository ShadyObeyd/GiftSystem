namespace GiftSystem.Models.ViewModels.Transactions
{
    public class DashboardReceivedTransactionsViewModel
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public string SenderUsername { get; set; }

        public int Credits { get; set; }
    }
}