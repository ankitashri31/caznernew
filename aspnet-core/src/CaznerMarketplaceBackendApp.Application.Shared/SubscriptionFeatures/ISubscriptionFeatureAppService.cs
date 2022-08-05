using Abp.Application.Services;
using CaznerMarketplaceBackendApp.SubscriptionFeatures.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubscriptionFeatures
{
    public interface ISubscriptionFeatureAppService : IAsyncCrudAppService<SubscriptionFeatureMasterDto, long, SubscriptionFeatureResultRequestDto, CreateOrUpdateSubscriptionFeatures, SubscriptionFeatureMasterDto>
    {
    }
}
