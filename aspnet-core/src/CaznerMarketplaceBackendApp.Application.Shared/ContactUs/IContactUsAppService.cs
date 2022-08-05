using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ContactUs.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ContactUs
{
   public  interface IContactUsAppService : IApplicationService
    {
        Task CreateContactUs(ContactUsDto Input);
    }
}
