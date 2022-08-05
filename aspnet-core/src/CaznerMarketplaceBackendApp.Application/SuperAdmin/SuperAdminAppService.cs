using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Users;
using CaznerMarketplaceBackendApp.Users.Dto;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Abp.Collections.Extensions;

namespace CaznerMarketplaceBackendApp.SuperAdmin

{
    [AbpAuthorize]
    public class SuperAdminAppService : CaznerMarketplaceBackendAppAppServiceBase, ISuperAdminAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserDetails, long> _userDetailsRepository;
        private IHostingEnvironment _Environment;
        private readonly IRepository<UserBusinessSettings, long> _userBusinessSettings;
        private IConfiguration _configuration;
        public SuperAdminAppService(IRepository<User, long> userRepository, IRepository<UserDetails, long> userDetailsRepository, IConfiguration configuration, IHostingEnvironment Environment, IRepository<UserBusinessSettings, long> userBusinessSettings)
        {
            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
            _configuration = configuration;
            _Environment = Environment;
            _userBusinessSettings = userBusinessSettings;
        }

        public async Task<UserListResponse> GetAllUsersPendingRequest(PagedResultRequestDto Input)
        {
            List<UserRequestDto> ResponseModel = new List<UserRequestDto>();
            int TotalCount = 0;
            bool IsLoadMore = false;
            UserListResponse response = new UserListResponse();

            try
            {
                var UserData = _userRepository.GetAllList(i => i.IsActive == false).ToList();
                var UserDetails = _userDetailsRepository.GetAll(); 
                ResponseModel = (from user in UserData
                                     join details in UserDetails on user.Id equals details.UserId
                                     where details.IsRequestApprovedByAdmin == false
                                     select new UserRequestDto
                                     {
                                         Id = user.Id,
                                         CompanyName = details.CompanyName,
                                         CreationDate = details.CreationTime,
                                         IsRejected = details.IsRequestRejectedByAdmin
                                     }).ToList();

                TotalCount = ResponseModel.Count();
                ResponseModel = ResponseModel.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                {
                    IsLoadMore = true;
                }
                response.SkipCount = Input.SkipCount;
                response.items = ResponseModel;
                response.TotalCount = TotalCount;
                response.IsLoadMore = IsLoadMore;
            }
            catch(Exception ex)
            {
            }

            return response;
        }

        public async Task<CustomerListResponse> GetAllUsersActiveRequest(UserCustomlistDto Input)
        {
            List<ApprovedUserModel> ResponseModel = new List<ApprovedUserModel>();

            int TotalCount = 0;
            bool IsLoadMore = false;
            CustomerListResponse response = new CustomerListResponse();
            try
            {
                var UserData = _userRepository.GetAllList(i => i.IsActive == true).ToList();
                var UserDetails = _userDetailsRepository.GetAll().WhereIf(!string.IsNullOrEmpty(Input.SearchText), x => x.CompanyName.ToLower().Trim().Contains(Input.SearchText.ToLower().Trim()));
                ResponseModel = (from user in UserData
                                 join details in UserDetails on user.Id equals details.UserId
                                 where details.IsRequestApprovedByAdmin== true
                                 select new ApprovedUserModel
                                 {
                                     Id = user.Id,
                                     CompanyName = details.CompanyName,
                                     AssociationDate = details.CreationTime,
                                     BusinessEmail = details.BusinessEmail,
                                     WebsiteRootUrl = details.WebsiteUrl,
                                     BusinessPhoneNumber = details.BusinessPhoneNumber,
                                     ApprovalDate = details.RequestApprovalDate
                                 }).ToList();

                TotalCount = ResponseModel.Count();
                ResponseModel = ResponseModel.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();
              
                if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                {
                    IsLoadMore = true;
                }
                response.SkipCount = Input.SkipCount;
                response.items = ResponseModel;
                response.TotalCount = TotalCount;
                response.IsLoadMore = IsLoadMore;

            }
            catch (Exception ex)
            {
            }

            return response;
        }

        public async Task<UserResponseDto> GetUserDetailsById(long UserId)
        {
            UserResponseDto Response = new UserResponseDto();
            try
            {
                var userDetails = _userDetailsRepository.GetAllList(i=> i.UserId == UserId).FirstOrDefault();
                var userData = _userRepository.GetAllList(i => i.Id == UserId).FirstOrDefault();

                if (userDetails != null)
                {
                    Response.UserId = userDetails.UserId;
                    Response.CompanyName = userDetails.CompanyName;
                    Response.BusinessTradingName = userDetails.BusinessTradingName;
                    Response.RegistrationBusinessNumber = userDetails.RegistrationBusinessNumber;
                    Response.BusinessPhoneNumber = userDetails.BusinessPhoneNumber;
                    Response.BusinessEmail = userDetails.BusinessEmail;
                    Response.PositionId = userDetails.PositionId;
                    Response.PositionName = userDetails.PositionMaster==null? "": userDetails.PositionMaster.PositionName;
                    Response.Title = userDetails.Title;
                    Response.WebsiteUrl = userDetails.WebsiteUrl;
                    Response.FirstName = userData.Name;
                    Response.SurName = userData.Surname;
                    Response.EmailAddress = userData.EmailAddress;
                    Response.PhoneNumber = userData.PhoneNumber;
                    Response.MobileNumber = userDetails.MobileNumber;
                    Response.StreetAddress = userDetails.StreetAddress;
                    Response.City = userDetails.City;
                    Response.StateId = userDetails.StateId;
                    Response.StateName = userDetails.State == null? "": userDetails.State.StateName;
                    Response.PostCode = userDetails.PostCode;
                    Response.CountryId = userDetails.CountryId;
                    Response.CountryName = userDetails.Country == null? "": userDetails.Country.CountryName;
                    Response.TimeZoneId = userDetails.TimeZoneId;
                    Response.CompanyComments = userDetails.CompanyComments;
                    Response.IsRejected = userDetails.IsRequestRejectedByAdmin;
                }
            }
            catch(Exception ex)
            {

            }
            return Response;
        }

        public async Task ApproveAndUpdateUserRequest(UserResponseDto userDetails)
        {
            try
            {
                var UserData = _userRepository.GetAllList(i => i.Id == userDetails.UserId).FirstOrDefault();
                if(UserData != null)
                {
                    UserData.IsActive = true;
                    await _userRepository.UpdateAsync(UserData);

                    var UserDetailsData = _userDetailsRepository.GetAllList(i => i.UserId == userDetails.UserId).FirstOrDefault();
                    if (UserDetailsData != null)
                    {
                        UserDetailsData.CompanyName = userDetails.CompanyName;
                        UserDetailsData.BusinessTradingName = userDetails.BusinessTradingName;
                        UserDetailsData.RegistrationBusinessNumber = userDetails.RegistrationBusinessNumber;
                        UserDetailsData.BusinessPhoneNumber = userDetails.BusinessPhoneNumber;
                        UserDetailsData.BusinessEmail = userDetails.BusinessEmail;
                        if (userDetails.PositionId.HasValue)
                        {
                            UserDetailsData.PositionId = userDetails.PositionId;
                        }
                        UserDetailsData.Title = userDetails.Title;
                        UserDetailsData.WebsiteUrl = userDetails.WebsiteUrl;
                        UserDetailsData.MobileNumber = userDetails.MobileNumber;
                        UserDetailsData.IsRequestApprovedByAdmin = true;
                        UserDetailsData.RequestApprovalDate = DateTime.UtcNow;
                        UserDetailsData.StreetAddress = userDetails.StreetAddress;
                        UserDetailsData.City = userDetails.City;
                        if (userDetails.StateId.HasValue)
                        {
                            UserDetailsData.StateId = userDetails.StateId;
                        }
                        UserDetailsData.PostCode = userDetails.PostCode;
                        if (userDetails.CountryId.HasValue)
                        {
                            UserDetailsData.CountryId = userDetails.CountryId;
                        }
                        UserDetailsData.TimeZoneId = userDetails.TimeZoneId;
                        UserDetailsData.CompanyComments = userDetails.CompanyComments;
                        await _userDetailsRepository.UpdateAsync(UserDetailsData);


                        //----------------------------------------------------

                        var pathToFile = _Environment.WebRootPath + "//EmailTemplates//RegistrationApproval.html";
                        var SupplierLogo = _userBusinessSettings.GetAllList(i => i.TenantId == userDetails.TenantId).Select(i => i.LogoUrl).FirstOrDefault();
                        // code to bind HTML content with replace tags
                        StreamReader ObjStream;
                        ObjStream = new StreamReader(pathToFile);
                        string Content = ObjStream.ReadToEnd();
                        ObjStream.Close();
                        //Content = Regex.Replace(Content, "{username}", UserData.EmailAddress);
                        //Content = Regex.Replace(Content, "{password}", UserData.Password);
                        Content = Regex.Replace(Content, "{name}", UserData.Name + " " + UserData.Surname);
                        if (!string.IsNullOrEmpty(SupplierLogo))
                        {
                            Content = Regex.Replace(Content, "{logo}", SupplierLogo);
                        }
                        Content = Regex.Replace(Content, "{body}", "Your registration request is approved by cazner admin. You can now access your portal.");
                        string EmailBody = Content;

                        //----------------------------------------------------
                        EmailHelper Emailhelper = new EmailHelper(_configuration);
                        #region send email
                        EmailDto email = new EmailDto();
                        email.EmailTo = UserData.EmailAddress;
                        email.Subject = "Your registration request approved successfully.";
                        email.Body = EmailBody;
                        await Emailhelper.SendEmail(email);

                        #endregion

                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        public async Task RejectUserApprovalRequest(long UserId)
        {
            try
            {
                var UserData = _userDetailsRepository.GetAllList(i => i.UserId == UserId).FirstOrDefault();
                if (UserData != null)
                {
                    UserData.IsRequestApprovedByAdmin = false;
                    UserData.IsRequestRejectedByAdmin = true;
                    UserData.RequestRejectionDate = DateTime.UtcNow;
                    await _userDetailsRepository.UpdateAsync(UserData);

                }
                var user =  _userRepository.GetAllList(i => i.Id == UserId).FirstOrDefault();
                //----------------------------------------------------

                var pathToFile = _Environment.WebRootPath + "//EmailTemplates//RegistrationReject.html";
                var SupplierLogo = _userBusinessSettings.GetAllList(i => i.TenantId == user.TenantId).Select(i => i.LogoUrl).FirstOrDefault();
                // code to bind HTML content with replace tags
                StreamReader ObjStream;
                ObjStream = new StreamReader(pathToFile);
                string Content = ObjStream.ReadToEnd();
                ObjStream.Close();
                Content = Regex.Replace(Content, "{name}", user.Name + " " + user.Surname);
                if (!string.IsNullOrEmpty(SupplierLogo))
                {
                    Content = Regex.Replace(Content, "{logo}", SupplierLogo);
                }
                Content = Regex.Replace(Content, "{body}", "Your registration request is rejected by cazner admin, you can contact cazner team for further support.");
                string EmailBody = Content;

                //----------------------------------------------------
                EmailHelper Emailhelper = new EmailHelper(_configuration);
                #region send email
                EmailDto email = new EmailDto();
                email.EmailTo = user.EmailAddress;
                email.Subject = "Your registration request rejected by cazner admin.";
                email.Body = EmailBody;
                await Emailhelper.SendEmail(email);

                #endregion


            }
            catch (Exception ex)
            {

            }

        }

        public async Task UpdateUserByDistributorId(UserResponseDto userDetails)
        {
            try
            {
                var UserData = _userDetailsRepository.GetAllList(i => i.UserId == userDetails.UserId).FirstOrDefault();
                var User = _userRepository.GetAllList(i => i.Id == userDetails.UserId).FirstOrDefault();
                if (User != null) {

                    User.Name = userDetails.FirstName;
                    User.Surname = userDetails.SurName;
                    User.EmailAddress = userDetails.EmailAddress;
                    User.PhoneNumber = userDetails.PhoneNumber;
                    await _userRepository.UpdateAsync(User);
                }
                if (UserData != null)
                {
                    UserData.UserId = userDetails.UserId;
                    UserData.CompanyName = userDetails.CompanyName;
                    UserData.BusinessTradingName = userDetails.BusinessTradingName;
                    UserData.RegistrationBusinessNumber = userDetails.RegistrationBusinessNumber;
                    UserData.BusinessPhoneNumber = userDetails.BusinessPhoneNumber;
                    UserData.BusinessEmail = userDetails.BusinessEmail;
                    if (userDetails.PositionId.HasValue)
                    {
                        UserData.PositionId = userDetails.PositionId;
                    }

                    UserData.Title = userDetails.Title;
                    UserData.WebsiteUrl = userDetails.WebsiteUrl;
                    UserData.MobileNumber = userDetails.MobileNumber;
                    UserData.StreetAddress = userDetails.StreetAddress;
                    UserData.City = userDetails.City;
                    if (userDetails.StateId.HasValue)
                    {
                        UserData.StateId = userDetails.StateId;
                    }
                    UserData.PostCode = userDetails.PostCode;
                    if (userDetails.CountryId.HasValue)
                    {
                        UserData.CountryId = userDetails.CountryId;
                    }
                    UserData.TimeZoneId = userDetails.TimeZoneId;
                    UserData.CompanyComments = userDetails.CompanyComments;
                    await _userDetailsRepository.UpdateAsync(UserData);

                }
            }
            catch (Exception ex)
            {

            }

        }
    } 
}
