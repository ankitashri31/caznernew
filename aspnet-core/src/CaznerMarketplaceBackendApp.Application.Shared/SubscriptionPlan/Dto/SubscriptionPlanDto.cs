using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.SubscriptionFeatures.Dto;
using CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubscriptionPlan.Dto
{
    public class SubscriptionPlanDto : EntityDto<long>
    {
        public string PlanTitle { get; set; }
        public string ShortDescription { get; set; }
        public double Amount { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public int UserType { get; set; }
        public string UserTypeName { get; set; }
        public int BillingTypeFrequency { get; set; }
        public string BillingTypeFrequencyName { get; set; }
        public bool IsMostPopularPlan { get; set; }

        public SubscriptionPlanFeatureDto[] FeatureModel { get; set; }
    }

    public class SubscriptionPlanResultRequestDto : PagedResultRequestDto
    {
      //  public string FeatureName { get; set; }
        public string Sorting { get; set; }

    }

    public class FeatureDetail
    {
        public List<SubscriptionPlanDto> PlanDetails { get; set; }

        public List<SubscriptionFeatureMasterDto> FeatureDetails { get; set; }
    }

    public class PlanCustomModel
    {
        public FeatureDetail SupplierModel { get; set; }

        public FeatureDetail DistributorModel { get; set; }
    }


}
