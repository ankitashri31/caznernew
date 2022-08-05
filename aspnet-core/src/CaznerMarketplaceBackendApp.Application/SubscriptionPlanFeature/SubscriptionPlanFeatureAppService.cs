using System;
using System.Collections.Generic;
using System.Text;
using CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments;

namespace CaznerMarketplaceBackendApp.SubscriptionPlanFeature
{
    public class SubscriptionPlanFeatureAppService :
         AsyncCrudAppService<SubscriptionPlanFeatures, SubscriptionPlanFeatureDto, long, SubscriptionPlanFeatureResultRequestDto, CreateOrUpdateSubscriptionPlanFeature, SubscriptionPlanFeatureDto>, ISubscriptionPlanFeatureAppService

    {
        public SubscriptionPlanFeatureAppService(IRepository<SubscriptionPlanFeatures, long> repository) : base(repository)
        {
        }
    



    }
}
