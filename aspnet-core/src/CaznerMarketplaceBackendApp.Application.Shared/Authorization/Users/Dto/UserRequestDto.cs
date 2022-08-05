using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Dto
{
    public class UserRequestDto
    {
        public long Id { get; set; }
        public string CompanyName {get; set;}
        public DateTime CreationDate { get; set; }
        public bool IsRejected { get; set; }

    }

    public class UserCustomlistDto : PagedResultRequestDto
    {
        public string SearchText { get; set; }
        public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<UserRequestDto> items { get; set; }
    }

    public class ApprovedUserModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime AssociationDate { get; set; }
        public string BusinessEmail { get; set; }
        public string WebsiteRootUrl { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }


    public class CustomerListResponse : PagedResultRequestDto
    {
        public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<ApprovedUserModel> items { get; set; }
    }
    public class UserListResponse : PagedResultRequestDto
    {
        public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<UserRequestDto> items { get; set; }
    }

}
