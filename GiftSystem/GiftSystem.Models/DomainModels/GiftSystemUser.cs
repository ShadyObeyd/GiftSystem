using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GiftSystem.Models.DomainModels
{
    
    public class GiftSystemUser : IdentityUser
    {
        const int DefaultCreditValue = 100;

        public GiftSystemUser()
        {
            this.Credits = DefaultCreditValue;
        }

        [Range(0, int.MaxValue)]
        public int Credits { get; set; }
    }
}