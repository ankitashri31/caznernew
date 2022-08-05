using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Artwork.Dto
{
   public class ArtworkMasterDto : EntityDto<long>
    {
        public string ArtworkUniqueId { get; set; }

        [Required]
        public string ArtworkSKU { get; set; }

        public string ArtworkFeeTitle { get; set; }
        public string ArtworkDescription { get; set; }

        public decimal UnitPrice { get; set; }
        public bool IsArtworkEnabled { get; set; }
        public decimal HandlingCharge { get; set; }
        public string ApprovalDescription { get; set; }
        public string ArtworkNote { get; set; }
        public bool IsEnableForMockups { get; set; }

        public string MockupSKU { get; set; }

        public string MockupUniqueId { get; set; }

        public string MockupTitle { get; set; }
        public string MockupDescription { get; set; }

        public decimal MockupPrice { get; set; }

        public double MaxNumberOfMockUpCanOrder { get; set; }
    }
}
