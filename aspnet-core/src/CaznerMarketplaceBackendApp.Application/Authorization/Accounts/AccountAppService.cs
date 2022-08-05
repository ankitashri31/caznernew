using System;
using System.Threading.Tasks;
using System.Web;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using CaznerMarketplaceBackendApp.Authorization.Accounts.Dto;
using CaznerMarketplaceBackendApp.Authorization.Impersonation;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Configuration;
using CaznerMarketplaceBackendApp.Debugging;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.Security.Recaptcha;
using CaznerMarketplaceBackendApp.Url;
using CaznerMarketplaceBackendApp.Authorization.Delegation;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.MultiTenancy.Dto;
using System.Linq;
using Abp.Domain.Uow;
using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Users;
using CaznerMarketplaceBackendApp.Users.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using WhoMasterDataCollection.ForgotPassword.Dto;

namespace CaznerMarketplaceBackendApp.Authorization.Accounts
{
    public class AccountAppService : CaznerMarketplaceBackendAppAppServiceBase, IAccountAppService
    {
        public IAppUrlService AppUrlService { get; set; }

        public IRecaptchaValidator RecaptchaValidator { get; set; }

        private readonly IUserEmailer _userEmailer;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<UserDetails, long> _userDetailsService;
        private readonly IWebUrlService _webUrlService;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IRepository<Tenant, int> _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<User, long> _userRepository;
        private IConfiguration _configuration;
        private IHostingEnvironment _Environment;
        private readonly UserManager _userManager;
        private readonly IRepository<UserBusinessSettings, long> _userBusinessSettings;
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        public AccountAppService(
            IUserEmailer userEmailer,
            UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IPasswordHasher<User> passwordHasher,
            IWebUrlService webUrlService,
            IUserDelegationManager userDelegationManager, IRepository<Tenant, int> tenantManager, IRepository<UserDetails, long> userDetailsService, IUnitOfWorkManager unitOfWorkManager, IConfiguration configuration, 
            IHostingEnvironment Environment, IRepository<UserBusinessSettings, long> userBusinessSettings, IRepository<User, long> userRepository, UserManager userManager)
        {
            _userEmailer = userEmailer;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _passwordHasher = passwordHasher;
            _webUrlService = webUrlService;

            AppUrlService = NullAppUrlService.Instance;
            RecaptchaValidator = NullRecaptchaValidator.Instance;
            _userDelegationManager = userDelegationManager;
            _tenantManager = tenantManager;
            _userDetailsService = userDetailsService;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            _Environment = Environment;
            _userBusinessSettings = userBusinessSettings;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id, _webUrlService.GetServerRootAddress(input.TenancyName));
        }

        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input.c);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                return Task.FromResult<int?>(null);
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }
         
        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            if (UseCaptchaOnRegistration())
            {
                await RecaptchaValidator.ValidateAsync(input.CaptchaResponse);
            }

            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                false,
                AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)   
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task RegisterTenantUser(CreateTenantRegistration input)
        {

            var userDetails = ObjectMapper.Map<UserDetails>(input);
            var PhoneNo = input.PhoneNumber.Substring(input.PhoneNumber.Length - 4);
            string UniqueDigit = RandomString(5);
            string TenancyName = input.Name + UniqueDigit;
            string UserName = TenancyName;
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                User ChkEmailAddress = await UserManager.FindByEmailAsync(input.AdminEmailAddress);

                if (ChkEmailAddress != null)
                {
                    throw new UserFriendlyException(L("ThisEmailAddressAlreadyExists"), L("ThisEmailAddressAlreadyExists_Detail"));
                }
            }

            await TenantManager.CreateWithAdminUserCustomAsync(TenancyName,
                input.Name,
                input.SurName,
                input.PhoneNumber,
                input.RoleName,
                UserName,
                userDetails,
                input.AdminPassword,
                input.AdminEmailAddress,
                null,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail,
                input.SubscriptionEndDateUtc?.ToUniversalTime(),
                input.IsInTrialPeriod,
                AppUrlService.CreateEmailActivationUrlFormat(TenancyName),
                input.SubscriptionPlanId
            );
        }

        public async Task RegisterFrontEndDistributorUser(CreateDistributorRegistration input)
        {
            try
            {
                var userDetails = ObjectMapper.Map<UserDetails>(input);
                var PhoneNo = input.PhoneNumber.Substring(input.PhoneNumber.Length - 4);
                string UniqueDigit = RandomString(5);
                string UserName = input.Name + UniqueDigit;
                string TenantId = string.Empty;
                Utility utility = new Utility();

                if (string.IsNullOrEmpty(input.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(input.EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }
                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {

                    User ChkEmailAddress = await UserManager.FindByEmailAsync(input.AdminEmailAddress);

                    if (ChkEmailAddress != null)
                    {
                        throw new UserFriendlyException(L("ThisEmailAddressAlreadyExists"), L("ThisEmailAddressAlreadyExists_Detail"));
                    }
                    string Password = await TenantManager.CreateDistributorUserAsync(Convert.ToInt32(TenantId),
                         input.Name,
                         input.SurName,
                         input.PhoneNumber,
                         input.RoleName,
                         UserName,
                         userDetails,
                         input.AdminPassword,
                         input.AdminEmailAddress,
                         null,
                         input.ShouldChangePasswordOnNextLogin,
                         input.SendActivationEmail,
                         input.SubscriptionEndDateUtc?.ToUniversalTime(),
                         input.IsInTrialPeriod,
                         AppUrlService.CreateEmailActivationUrlFormat(Convert.ToInt32(TenantId)),
                         input.SubscriptionPlanId
                     );

                    try
                    {

                        var pathToFile = _Environment.WebRootPath + "//EmailTemplates//RegistrationSuccess.html";
                        var SupplierLogo = _userBusinessSettings.GetAllList(i => i.TenantId == Convert.ToInt32(TenantId)).Select(i => i.LogoUrl).FirstOrDefault();
                        // code to bind HTML content with replace tags
                        StreamReader ObjStream;
                        ObjStream = new StreamReader(pathToFile);
                        string Content = ObjStream.ReadToEnd();
                        ObjStream.Close();
                        Content = Regex.Replace(Content, "{username}", input.AdminEmailAddress);
                        Content = Regex.Replace(Content, "{password}", Password);
                        Content = Regex.Replace(Content, "{name}", input.Name + " " + input.SurName);
                        if (!string.IsNullOrEmpty(SupplierLogo))
                        {
                            Content = Regex.Replace(Content, "{logo}", SupplierLogo);
                        }
                        Content = Regex.Replace(Content, "{body}", "Your registration request has been sent to cazner orso admin, you will soon receive an email for your account activation. You can refer below credentials to login once account is activated:");
                        string EmailBody = Content;

                        //----------------------------------------------------
                        EmailHelper Emailhelper = new EmailHelper(_configuration);
                        #region send email
                        EmailDto email = new EmailDto();
                        email.EmailTo = input.AdminEmailAddress;
                        email.Subject = "Your registration request received to cazner.";
                        email.Body = EmailBody;
                        await Emailhelper.SendEmail(email);
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            { }
            
        }

        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
                var user = await GetUserByChecking(input.EmailAddress);
                user.SetNewPasswordResetCode();
                await _userEmailer.SendPasswordResetLinkAsync(
                    user,
                    AppUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
                    );
            
        }

        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive,
                UserName = user.UserName
            };
        }


        public async Task<string> SendForgotPasswordLinkToUser(ForgotPasswordDto Model)
        {
            string token = string.Empty;
            string ForgorPwdURL = string.Empty;
            string IsSuccess = string.Empty;
            try
            {
               // string ApplicationURL = _configuration.GetValue<string>("App:ClientRootAddress");
                string ApplicationURL = _configuration.GetValue<string>("App:FrontendForgotPwdLink");
                User user = new User();

                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    //var userData = await _userManager.FindByEmailAsync(Model.Email.Trim());
                    user = await _userManager.FindByEmailAsync(Model.Email.Trim());
                }

                if (user != null)
                {
                    // Generates a password reset token for the user
                    token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    user.PasswordResetCode = token;
                    user.LastModificationTime = DateTime.UtcNow;
                    await CurrentUnitOfWork.SaveChangesAsync();


                    string EncryptedUserId = EnryptString(user.Id.ToString());
                    //string URL = ApplicationURL + WebUtility.UrlEncode(EncryptedUserId) + "/" + WebUtility.UrlEncode(token);
                    string URL = ApplicationURL + EncryptedUserId + "/" + token;
                    var pathToFile = _Environment.WebRootPath + "\\EmailTemplates\\ForgotPassword.html";

                    // code to bind HTML content with replace tags
                    StreamReader ObjStream;
                    ObjStream = new StreamReader(pathToFile);
                    string Content = ObjStream.ReadToEnd();
                    ObjStream.Close();
                    Content = Regex.Replace(Content, "{name}", user.Name +" "+ user.Surname);
                    Content = Regex.Replace(Content, "{URL}", URL);
                   // Content = Regex.Replace(Content, "{body}", "Please click here to reset your password");
                    string EmailBody = Content;

                    //----------------------------------------------------
                    EmailHelper Emailhelper = new EmailHelper(_configuration);
                    #region send email
                    EmailDto email = new EmailDto();
                    email.EmailTo = user.EmailAddress;
                    email.Subject = "Cazner Reset your password";
                    email.Body = EmailBody;
                    await Emailhelper.SendEmail(email);
                    #endregion

                }
                else
                {
                    IsSuccess = "Failure";
                }
            }
            catch (Exception ex)
            {
                Logger.Error("INFO: " + DateTime.UtcNow + " || Error: " + ex.StackTrace);
            }
            return IsSuccess;
        }

        public async Task<string> UpdateForgotPassword(ForgotPasswordDto Model)
        {
            string IsSuccess = string.Empty;
            string Token = Model.Token;   //*/ WebUtility.UrlDecode(Model.Token);
            string DecryptedUserId = DecryptString(Model.UserId);
            if (!string.IsNullOrEmpty(DecryptedUserId))
            {
                User user = new User();
                int TenantId = 0;
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    //user = _userRepository.GetAllList(i => i.Id == Convert.ToInt64(DecryptedUserId)).FirstOrDefault();
                    user = await _userManager.FindByIdAsync(DecryptedUserId);
                    TenantId = user.TenantId.Value;

                }

                using (CurrentUnitOfWork.SetTenantId(TenantId))
                {
                    // password complexity validation
                    if (!new Regex(AccountAppService.PasswordRegex).IsMatch(Model.NewPassword))
                    {
                        throw new UserFriendlyException("Passwords should be at least one non alphanumeric, Including a lowercase, uppercase, and number.");
                    }
                    try
                    {
                        var ChangePassword = await _userManager.ResetPasswordAsync(user, Token, Model.NewPassword);
                        user.PasswordResetCode = null;
                        user.LastModificationTime = DateTime.UtcNow;
                        await CurrentUnitOfWork.SaveChangesAsync();
                        IsSuccess = "Success";
                    }
                    catch (Exception ex)
                    {
                        IsSuccess = "InValid Token";
                    }
                   
                }
            }
            return IsSuccess;
        }

        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            user.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(
                user,
                AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
            );
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user != null && user.IsEmailConfirmed)
            {
                return;
            }

            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != input.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await UserManager.UpdateAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }

        public virtual async Task<ImpersonateOutput> DelegatedImpersonate(DelegatedImpersonateInput input)
        {
            var userDelegation = await _userDelegationManager.GetAsync(input.UserDelegationId);
            if (userDelegation.TargetUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("User delegation error.");
            }

            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(userDelegation.SourceUserId, userDelegation.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(userDelegation.TenantId)
            };
        }

        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

        public virtual async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken = await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TargetTenantId)
            };
        }

        private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        public async Task<List<TenantData>> GetTenantData()
        {
            List<TenantData> TenantList = new List<TenantData>();
            Utility utility = new Utility();
            try
            {
                // using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    TenantList = (from tenant in _tenantManager.GetAll()
                                  join user in _userDetailsService.GetAll() on tenant.Id equals user.TenantId
                                  select new TenantData
                                  {
                                      TenantId = tenant.Id,
                                      TenantName = tenant.TenancyName,
                                      SiteUrl = user.WebsiteUrl,
                                      EncryptedTenantId = utility.EnryptString(tenant.Id.ToString())
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return TenantList;
        }



        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            User user = new User();
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                user = await UserManager.FindByEmailAsync(inputEmailAddress);
                if (user == null)
                {
                    throw new UserFriendlyException(L("InvalidEmailAddress"));
                }
            }

            return user;
        }

        private string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        private string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }
    }

    public class TenantData
    {
        public string SiteUrl { get; set; }
        public long TenantId { get; set; }
        public string TenantName { get; set; }
        public string EncryptedTenantId { get; set; }
    }
}