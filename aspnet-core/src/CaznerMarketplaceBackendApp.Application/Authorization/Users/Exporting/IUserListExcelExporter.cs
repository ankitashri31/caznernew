using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}