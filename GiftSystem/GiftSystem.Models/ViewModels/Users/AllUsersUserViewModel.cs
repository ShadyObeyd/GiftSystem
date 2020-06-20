namespace GiftSystem.Models.ViewModels.Users
{
    public class AllUsersUserViewModel
    {
        public string Username { get; set; }

        public int Credits { get; set; }

        public int SentTransactionsCount { get; set; }

        public int ReceivedTransactionsCount { get; set; }
    }
}