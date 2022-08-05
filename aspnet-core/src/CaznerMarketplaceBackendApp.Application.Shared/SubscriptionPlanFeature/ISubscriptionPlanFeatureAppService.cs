using System;
using System.Collections.Generic;
using System.Text;
using CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace CaznerMarketplaceBackendApp.SubscriptionPlanFeature
{
    public interface ISubscriptionPlanFeatureAppService : IAsyncCrudAppService<SubscriptionPlanFeatureDto, long, SubscriptionPlanFeatureResultRequestDto, CreateOrUpdateSubscriptionPlanFeature, SubscriptionPlanFeatureDto>
    {
    
    }
}
