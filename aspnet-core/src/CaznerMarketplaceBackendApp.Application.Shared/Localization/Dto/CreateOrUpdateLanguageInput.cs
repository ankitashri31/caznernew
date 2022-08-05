using System.ComponentModel.DataAnnotations;

namespace CaznerMarketplaceBackendApp.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}