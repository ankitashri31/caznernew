using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using CaznerMarketplaceBackendApp.Country;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Position;
using Abp.Authorization.Users;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public class UserDetails : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        public string BusinessEmail { get; set; }
  
        public string CompanyName { get; set; }
        public string BusinessTradingName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string BusinessPhoneNumber { get; set; }

        public string RegistrationBusinessNumber { get; set; }

        public virtual PositionMaster PositionMaster { get; set; }
        [ForeignKey("PositionMaster")]
        public long? PositionId { get; set; }
        public string Title { get; set; }
        public string WebsiteUrl { get; set; }
     

        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public virtual States State { get; set; }
        [ForeignKey("State")]
        public long? StateId { get; set; }

        public virtual Countries Country { get; set; }
        [ForeignKey("Country")]
        public long? CountryId { get; set; }

        public string PostCode { get; set; }
        public string PONumber { get; set; }

        public string CompanyPrivateEmail { get; set; }

        public string TimeZoneId { get; set; }

        public string CompanyComments { get; set; }

        public string CompanyPhoneNumber { get; set; }
        public string CompanyPublicEmail { get; set; }
        public bool Status { get; set; }

        public DateTime? RequestApprovalDate { get; set; }
        public bool IsRequestApprovedByAdmin { get; set; }
        public bool IsRequestRejectedByAdmin { get; set; }

        public DateTime? RequestRejectionDate { get; set; }
    }
}
