using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
