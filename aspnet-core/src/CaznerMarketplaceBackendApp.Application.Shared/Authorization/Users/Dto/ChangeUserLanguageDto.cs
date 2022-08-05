using System.ComponentModel.DataAnnotations;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
