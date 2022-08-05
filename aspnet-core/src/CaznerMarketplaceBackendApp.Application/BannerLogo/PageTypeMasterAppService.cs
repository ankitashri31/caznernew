using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.BannerLogo.Dto;
using FimApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.BannerLogo
{
    public class PageTypeMasterAppService :
         AsyncCrudAppService<BannerPageTypeMaster, BannerPageTypeMasterDto, long, BannerLogoResultRequestDto, CreateOrUpdateBannerLogo, BannerPageTypeMasterDto>, IPageTypeMasterAppService
    {
        private readonly IRepository<BannerPageTypeMaster, long> _repository;
        private readonly IRepository<UserBannerLogoData, long> _repositoryUserBannerLogoData;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public PageTypeMasterAppService(IRepository<BannerPageTypeMaster, long> repository, IRepository<UserBannerLogoData, long> repositoryUserBannerLogoData, IUnitOfWorkManager unitOfWorkManager) : base(repository)
        {
            _repository = repository;
            _repositoryUserBannerLogoData = repositoryUserBannerLogoData;
            _unitOfWorkManager = unitOfWorkManager;
        }
        public async Task DeleteByIdAsync(long Id)
        {
            var UserlogoData = _repositoryUserBannerLogoData.GetAll().Where(i => i.PageTypeId == Id).ToList();

            if (UserlogoData.Count > 0)
            {
                await DeleteUserBannerDataAsync(UserlogoData, AbpSession.TenantId.Value);

            }

            var pagetype = Repository.GetAll().Where(i => i.Id == Id).FirstOrDefault();

            if (pagetype != null)
            {
                await _repository.DeleteAsync(pagetype);

            }
        }
        private async Task<bool> DeleteUserBannerDataAsync(List<UserBannerLogoData> UserBannerLogoData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryUserBannerLogoData.BulkDelete(UserBannerLogoData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
    }
}