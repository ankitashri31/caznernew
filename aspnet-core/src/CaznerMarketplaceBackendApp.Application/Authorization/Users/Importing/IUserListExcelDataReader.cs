using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
