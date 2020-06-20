namespace GiftSystem.Models.ViewModels.Transactions
{
    public class DetailsTransactionViewModel
    {
        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public int Credits { get; set; }

        public string Wish { get; set; }
    }
}