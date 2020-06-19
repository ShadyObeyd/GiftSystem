using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftSystem.Models.DomainModels
{
    public class GiftSystemUser : IdentityUser
    {
        const int DefaultCreditValue = 100;

        public GiftSystemUser()
        {
            this.Credits = DefaultCreditValue;
            this.SentTransactions = new HashSet<Transaction>();
            this.ReceivedTransactions = new HashSet<Transaction>();
        }

        [Range(0, int.MaxValue)]
        public int Credits { get; set; }

        public IEnumerable<Transaction> SentTransactions { get; set; }

        public IEnumerable<Transaction> ReceivedTransactions { get; set; }
    }
}