using GiftSystem.Models.InputModels.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftSystem.Models.InputModels.Transactions
{
    public class CreateTransactionInputModel
    {
        private const string MissingRecieverMessage = "There must be a reciever in order to send a gift!";
        private const string PhoneNumberDisplayName = "Receiver's Phone Number";
        private const string CreditsDisplayName = "Amount of credits to send";
        private const string WishDisplayName = "Wish something to your friend";

        public string SenderId { get; set; }

        [Required(ErrorMessage = MissingRecieverMessage)]
        public string ReceiverId { get; set; }

        [Required]
        [Phone]
        [Display(Name = PhoneNumberDisplayName)]
        public string RecieverPhoneNumber { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = CreditsDisplayName)]
        public int Credits { get; set; }

        [Display(Name = WishDisplayName)]
        public string Wish { get; set; }

        public IEnumerable<CreateTransactionUserInputModel> Users { get; set; }
    }
}