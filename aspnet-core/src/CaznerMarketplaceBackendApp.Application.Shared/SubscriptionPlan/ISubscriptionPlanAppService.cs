using Abp.Application.Services;
using CaznerMarketplaceBackendApp.SubscriptionPlan.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubscriptionPlan
{
    public interface ISubscriptionPlanAppService : IAsyncCrudAppService<SubscriptionPlanDto, long, SubscriptionPlanResultRequestDto, CreateOrUpdateSubscriptionPlan, SubscriptionPlanDto>
    {
    }
}
