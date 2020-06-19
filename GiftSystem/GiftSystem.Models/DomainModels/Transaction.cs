using System.ComponentModel.DataAnnotations.Schema;

namespace GiftSystem.Models.DomainModels
{
    public class Transaction
    {
        public string SenderId { get; set; }

        public GiftSystemUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public GiftSystemUser Receiver { get; set; }

        public int Credits { get; set; }
    }
}