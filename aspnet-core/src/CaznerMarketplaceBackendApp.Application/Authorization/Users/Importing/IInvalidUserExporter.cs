using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Authorization.Users.Importing.Dto;
using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
