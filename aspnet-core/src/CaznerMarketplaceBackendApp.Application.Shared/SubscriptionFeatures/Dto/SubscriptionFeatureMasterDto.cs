using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubscriptionFeatures.Dto
{
    public class SubscriptionFeatureMasterDto : EntityDto<long>
    {
        public string FeatureTitle { get; set; }
        public Guid FeatureUniqueId { get; set; }
        public bool IsActive { get; set; }

        public List<SubscriptionPlanFeatureDto> FeaturePlanDetails { get; set; }
    }

    public class FeaturePlan
    {
        public SubscriptionFeatureMasterDto FeatureDetails { get; set; }

    }


    public class SubscriptionFeatureResultRequestDto : PagedResultRequestDto
    {
        public string FeatureName { get; set; }
        public string Sorting { get; set; }

    }
}
