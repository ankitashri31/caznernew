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
    public class ImageTypeMasterAppService :
         AsyncCrudAppService<ImageTypeMaster, ImageTypeMasterDto, long, ImageTypeMasterResultRequestDto, CreateOrUpdateImageTypeMaster, ImageTypeMasterDto>, IImageTypeMasterAppService
    {
        private readonly IRepository<ImageTypeMaster, long> _repository;
        private readonly IRepository<UserBannerLogoData, long> _repositoryUserBannerLogoData;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ImageTypeMasterAppService(IRepository<ImageTypeMaster, long> repository, IRepository<UserBannerLogoData, long> repositoryUserBannerLogoData, IUnitOfWorkManager unitOfWorkManager) : base(repository)
        {
            _repository = repository;
            _repositoryUserBannerLogoData = repositoryUserBannerLogoData;
            _unitOfWorkManager = unitOfWorkManager;
        }
        public async Task DeleteByIdAsync(long Id)
        {
            var UserlogoData = _repositoryUserBannerLogoData.GetAll().Where(i => i.ImageTypeId == Id).ToList();

            if (UserlogoData.Count > 0)
            {
                await DeleteImageTypeDataAsync(UserlogoData, AbpSession.TenantId.Value);
            }

            var Imagetype = Repository.GetAll().Where(i => i.Id == Id).FirstOrDefault();

            if (Imagetype != null)
            {
                await _repository.DeleteAsync(Imagetype);

            }
        }
        private async Task<bool> DeleteImageTypeDataAsync(List<UserBannerLogoData> UserBannerLogoData, int TenantId)
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
