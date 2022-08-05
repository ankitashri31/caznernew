using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public interface IUserDetailsAppService : IApplicationService
    {
        //Task<UserResponseDto> GetLoggedInUserDataById();
    }
}
