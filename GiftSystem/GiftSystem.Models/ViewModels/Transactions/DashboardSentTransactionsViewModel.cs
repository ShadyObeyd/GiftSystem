namespace GiftSystem.Models.ViewModels.Transactions
{
    public class DashboardSentTransactionsViewModel
    {
        public string Id { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverUsername { get; set; }

        public int Credits { get; set; }
    }
}