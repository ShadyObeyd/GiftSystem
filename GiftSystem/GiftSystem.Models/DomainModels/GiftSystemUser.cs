using Microsoft.AspNetCore.Identity;

namespace GiftSystem.Models.DomainModels
{
    
    public class GiftSystemUser : IdentityUser
    {
        const int DefaultCreditValue = 100;

        public GiftSystemUser()
        {
            this.Credits = DefaultCreditValue;
        }

        public int Credits { get; set; }
    }
}