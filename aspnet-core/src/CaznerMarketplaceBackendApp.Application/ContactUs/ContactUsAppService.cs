using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.ContactUs.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Users;
using Microsoft.AspNetCore.Hosting;
using Abp.Authorization;
using System.Text.RegularExpressions;
using CaznerMarketplaceBackendApp.Users.Dto;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CaznerMarketplaceBackendApp.ContactUs
{
    public class ContactUsAppService : CaznerMarketplaceBackendAppAppServiceBase, IContactUsAppService
    {
        private readonly IRepository<ContactUsMaster, long> _contactUsMasterRepository;
        private readonly IRepository<UserBusinessSettings, long> _UserBusinessSettingsRepository;

        private IHostingEnvironment _Environment;
        private IConfiguration _configuration;
        public ContactUsAppService
              (IRepository<ContactUsMaster, long> contactUsMasterRepository, IRepository<UserBusinessSettings, long> UserBusinessSettingsRepository, IHostingEnvironment Environment, IConfiguration configuration)
        {
            _contactUsMasterRepository = contactUsMasterRepository;
            _UserBusinessSettingsRepository = UserBusinessSettingsRepository;
            _Environment = Environment;
            _configuration = configuration;
        }
        public async Task CreateContactUs(ContactUsDto Input)
        {            
            try
            {
                string TenantId = string.Empty;
                Utility utility = new Utility();
                if (string.IsNullOrEmpty(Input.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Input.EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }
                if (!string.IsNullOrEmpty(TenantId))
                {
                    var contactUsMaster = ObjectMapper.Map<ContactUsMaster>(Input);
                    var resultUniqueId = await _contactUsMasterRepository.InsertAndGetIdAsync(contactUsMaster);
                    var BusinessSettingsData = _UserBusinessSettingsRepository.GetAllList(i => i.TenantId == Convert.ToInt32(TenantId)).FirstOrDefault();

                    #region send email to registered business email address (business settings page)
                    var pathToFile = _Environment.WebRootPath + "\\EmailTemplates\\ContactUs.html";

                    // code to bind HTML content with replace tags
                    if (BusinessSettingsData != null)
                    {
                        StreamReader ObjStream;
                        ObjStream = new StreamReader(pathToFile);
                        string Content = ObjStream.ReadToEnd();
                        ObjStream.Close();
                        Content = Regex.Replace(Content, "{adminname}", BusinessSettingsData.FirstName +" "+ BusinessSettingsData.LastName);
                        Content = Regex.Replace(Content, "{body}", "You have received below query from application user:");
                        Content = Regex.Replace(Content, "{username}", contactUsMaster.Name);
                        Content = Regex.Replace(Content, "{useremailaddress}", contactUsMaster.EmailAddress);
                        Content = Regex.Replace(Content, "{message}", contactUsMaster.MessageBody);
                        string EmailBody = Content;

                        //----------------------------------------------------
                        EmailHelper Emailhelper = new EmailHelper(_configuration);
                        #region send email
                        EmailDto email = new EmailDto();
                        email.EmailTo = BusinessSettingsData.BusinessEmail;
                        email.Subject = contactUsMaster.MessageSubject;
                        email.Body = EmailBody;
                        await Emailhelper.SendEmail(email);
                        #endregion


                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }           
        }
    }
}
