using Abp.Authorization;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using CaznerMarketplaceBackendApp.BusinessSetting.Dto;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.MultiTenancy.Dto;
using CaznerMarketplaceBackendApp.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    [AbpAuthorize]
    public class UserDetailsAppService : CaznerMarketplaceBackendAppAppServiceBase, IUserDetailsAppService
    {
        private readonly IRepository<UserDetails, long> _userDetailsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserShippingAddress, long> _userShippingAddressRepository;

        public UserDetailsAppService(IRepository<UserDetails, long> userDetailsRepository, IRepository<User, long> userRepository, IRepository<UserShippingAddress, long> userShippingAddressRepository)
        {
            _userDetailsRepository = userDetailsRepository;
            _userRepository = userRepository;
            _userShippingAddressRepository = userShippingAddressRepository;


        }

        public async Task<UserShippingAddressDto> GetLoggedInUserDataById()
        {
            long UserId = AbpSession.UserId.Value;
            UserShippingAddressDto Response = new UserShippingAddressDto();
            try
            {
                var User = _userRepository.GetAll().Where(i => i.Id == UserId);
                var UserDetails = _userDetailsRepository.GetAll().Where(i => i.UserId == UserId);
                Response = (from user in User
                            join detail in UserDetails on user.Id equals detail.UserId
                            select new UserShippingAddressDto
                            {
                                Id = detail.Id,
                                StateId = detail.StateId,
                                FirstName = user.Name,
                                LastName =user.Surname,
                                CountryId =detail.CountryId,
                                City =detail.City,
                                StreetAddress = detail.StreetAddress,
                                PostCode =detail.PostCode,
                                BusinessEmail =detail.BusinessEmail,
                                CompanyName = detail.CompanyName,
                                MobileNumber = detail.MobileNumber,
                                StateName =detail.State.StateName,
                                CountryName =detail.Country.CountryName

                            }).FirstOrDefault();

            }
            catch(Exception ex)
            {

            }
            return Response;
        }

        public async Task UpdateProfileDetails(UserShippingAddressDto Input)
        {
            try
            {
                var UserDetails = _userDetailsRepository.Get(Input.Id);
                if(UserDetails!=null)
                {
                    var user = _userRepository.Get(UserDetails.UserId);

                    if(!string.IsNullOrEmpty(Input.FirstName))
                    {
                        user.Name = Input.FirstName;
                    }
                    if (!string.IsNullOrEmpty(Input.LastName))
                    {
                        user.Surname = Input.LastName;
                    }
                    if (!string.IsNullOrEmpty(Input.MobileNumber))
                    {
                        user.PhoneNumber = Input.MobileNumber;
                    }
                    await _userRepository.UpdateAsync(user);

                    UserDetails.City = Input.City;
                    UserDetails.StateId = Input.StateId;
                    UserDetails.CountryId = Input.CountryId;
                    UserDetails.PostCode = Input.PostCode;
                    UserDetails.CompanyName = Input.CompanyName;
                    UserDetails.BusinessEmail = Input.BusinessEmail;
                    UserDetails.StreetAddress = Input.StreetAddress;
                    UserDetails.PONumber = Input.PONumber;
                    await _userDetailsRepository.UpdateAsync(UserDetails);
                }
            }
            catch(Exception ex)
            {

            }
        }
   }
}
