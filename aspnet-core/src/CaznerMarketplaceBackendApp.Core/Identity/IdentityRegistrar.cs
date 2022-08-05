using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using CaznerMarketplaceBackendApp.Authentication.TwoFactor.Google;
using CaznerMarketplaceBackendApp.Authorization;
using CaznerMarketplaceBackendApp.Authorization.Roles;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Editions;
using CaznerMarketplaceBackendApp.MultiTenancy;

namespace CaznerMarketplaceBackendApp.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
                {
                    options.Tokens.ProviderMap[GoogleAuthenticatorProvider.Name] = new TokenProviderDescriptor(typeof(GoogleAuthenticatorProvider));
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
