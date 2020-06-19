using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftSystem.Models.DomainModels
{
    public class Transaction
    {
        private const string WishDisplayName = "Wish something to your friend";

        public Transaction()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public string SenderId { get; set; }

        public GiftSystemUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public GiftSystemUser Receiver { get; set; }

        [Range(0, int.MaxValue)]
        public int Credits { get; set; }

        public string Wish { get; set; }
    }
}