namespace GiftSystem.Models.ViewModels.Transactions
{
    public class AllTransactionsViewModel
    {
        public string Id { get; set; }

        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public int Credits { get; set; }
    }
}