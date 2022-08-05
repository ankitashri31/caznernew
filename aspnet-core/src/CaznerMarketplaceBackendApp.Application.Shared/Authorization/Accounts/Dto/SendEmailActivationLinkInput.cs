using System.ComponentModel.DataAnnotations;

namespace CaznerMarketplaceBackendApp.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}