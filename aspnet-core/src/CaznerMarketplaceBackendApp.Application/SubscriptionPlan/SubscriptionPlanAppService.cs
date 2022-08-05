using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments;
using CaznerMarketplaceBackendApp.SubscriptionFeatures.Dto;
using CaznerMarketplaceBackendApp.SubscriptionPlan.Dto;
using CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.SubscriptionPlan
{
    public class SubscriptionPlanAppService :
         AsyncCrudAppService<SubscriptionPlanMaster, SubscriptionPlanDto, long, SubscriptionPlanResultRequestDto, CreateOrUpdateSubscriptionPlan, SubscriptionPlanDto>, ISubscriptionPlanAppService

    {
        public readonly IRepository<SubscriptionFeaturesMaster, long> _subscriptionFeatureApp;
        public readonly IRepository<SubscriptionPlanFeatures, long> _subscriptionPlanFeaturesApp;
        public SubscriptionPlanAppService(IRepository<SubscriptionPlanMaster, long> repository, IRepository<SubscriptionFeaturesMaster, long> subscriptionFeatureApp, IRepository<SubscriptionPlanFeatures, long> subscriptionPlanFeaturesApp) : base(repository)
        {
            _subscriptionFeatureApp = subscriptionFeatureApp;
            _subscriptionPlanFeaturesApp = subscriptionPlanFeaturesApp;
        }

        public async Task <PlanCustomModel> GetSubscriptionPlanData()
        {
            PlanCustomModel ResponseModel = new PlanCustomModel();
            FeatureDetail SupplierFeatures = new FeatureDetail();
            FeatureDetail DistributorFeatures = new FeatureDetail();
            List<SubscriptionPlanDto> SupplierPlanDto = new List<SubscriptionPlanDto>();
            List<SubscriptionFeatureMasterDto> SupplierFeaturePlanDto = new List<SubscriptionFeatureMasterDto>();
            List<SubscriptionFeatureMasterDto> DistributorFeaturePlanDto = new List<SubscriptionFeatureMasterDto>();

            try
            {
                var PlanData = Repository.GetAll();

                SupplierPlanDto = (from plan in PlanData
                           select new SubscriptionPlanDto
                           {
                               Id = plan.Id,
                               ShortDescription = plan.ShortDescription,
                               Amount = plan.Amount,
                               BillingTypeFrequency = plan.BillingTypeFrequency,
                               BillingTypeFrequencyName = plan.BillingTypeFrequency == 1 ? "Monthly" : "Yearly",
                               CurrencyType = plan.CurrencyType,
                               CurrencySymbol= plan.CurrencySymbol==1?"$":"",
                               CurrencyName = plan.CurrencyType == 1 ? "USD" : "$",
                               IsMostPopularPlan = plan.IsMostPopularPlan,
                               PlanTitle = plan.PlanTitle,
                               UserType = plan.UserType,
                               UserTypeName = plan.UserType == 1 ? "Supplier" : "Distributor"
                           }).OrderBy(x=>x.Id).ToList();



                SupplierFeaturePlanDto = (from feature in _subscriptionFeatureApp.GetAll().Distinct()
                                          join featureplan in _subscriptionPlanFeaturesApp.GetAll() on feature.Id equals featureplan.SubscriptionFeatureId
                                          join plan in PlanData.Where(i => i.UserType == 1) on featureplan.SubscriptionPlanId equals plan.Id
                                          where plan.UserType==1
                                          select new SubscriptionFeatureMasterDto
                                          {
                                              Id = feature.Id,
                                              FeatureTitle = feature.FeatureTitle,
                                              FeatureUniqueId = feature.FeatureUniqueId,
                                              IsActive = feature.IsActive,
                                              FeaturePlanDetails = (from featureplan in _subscriptionPlanFeaturesApp.GetAll()
                                                             join plans in PlanData.Where(i => i.UserType == 1) on featureplan.SubscriptionPlanId equals plans.Id
                                                             where featureplan.SubscriptionFeatureId == feature.Id && plan.UserType == 1
                                                             select new SubscriptionPlanFeatureDto
                                                             {
                                                                 Id = featureplan.Id,
                                                                 SubscriptionPlanId = featureplan.SubscriptionPlanId,
                                                                 AllowedQuantity = featureplan.AllowedQuantity,
                                                                 FreeText = string.IsNullOrEmpty(featureplan.FreeText)?"": featureplan.FreeText,
                                                                 IsAccessAllowed = featureplan.IsAccessAllowed,
                                                                 IsFreeTextExists = featureplan.IsFreeTextExists,
                                                                 SubscriptionFeatureId = featureplan.SubscriptionFeatureId
                                                             }).OrderBy(x=>x.SubscriptionPlanId).ToList()

                                          }).Distinct().ToList();
                SupplierFeatures.PlanDetails = SupplierPlanDto.Where(i=>i.UserType==(int)SubscriptionUserType.Supplier).ToList();
                SupplierFeatures.FeatureDetails = SupplierFeaturePlanDto;

                DistributorFeaturePlanDto = (from feature in _subscriptionFeatureApp.GetAll().Distinct()
                                             join featureplan in _subscriptionPlanFeaturesApp.GetAll() on feature.Id equals featureplan.SubscriptionFeatureId
                                             join plan in PlanData.Where(i=>i.UserType== 2) on featureplan.SubscriptionPlanId equals plan.Id
                                             //where plan.UserType == (int)SubscriptionUserType.Distributor
                                             select new SubscriptionFeatureMasterDto
                                          {
                                              Id = feature.Id,
                                              FeatureTitle = feature.FeatureTitle,
                                              FeatureUniqueId = feature.FeatureUniqueId,
                                              IsActive = feature.IsActive,
                                              FeaturePlanDetails = (from featureplan in _subscriptionPlanFeaturesApp.GetAll()
                                                             join plans in PlanData.Where(i => i.UserType == 2) on featureplan.SubscriptionPlanId equals plans.Id
                                                             where featureplan.SubscriptionFeatureId == feature.Id && plan.UserType == 2
                                                             select new SubscriptionPlanFeatureDto
                                                             {
                                                                 Id = featureplan.Id,
                                                                 SubscriptionPlanId = featureplan.SubscriptionPlanId,
                                                                 AllowedQuantity = featureplan.AllowedQuantity,
                                                                 FreeText = string.IsNullOrEmpty(featureplan.FreeText) ? "" : featureplan.FreeText,
                                                                 IsAccessAllowed = featureplan.IsAccessAllowed,
                                                                 IsFreeTextExists = featureplan.IsFreeTextExists,
                                                                 SubscriptionFeatureId = featureplan.SubscriptionFeatureId
                                                             }).OrderBy(x => x.SubscriptionPlanId).ToList()

                                          }).Distinct().ToList();

                DistributorFeatures.PlanDetails = SupplierPlanDto.Where(i => i.UserType == (int)SubscriptionUserType.Distributor).ToList();
                DistributorFeatures.FeatureDetails = DistributorFeaturePlanDto;

                ResponseModel.SupplierModel = SupplierFeatures;
                ResponseModel.DistributorModel = DistributorFeatures;
            }
            catch (Exception ex)
            {

            }
            return ResponseModel;
        }
    }
}
