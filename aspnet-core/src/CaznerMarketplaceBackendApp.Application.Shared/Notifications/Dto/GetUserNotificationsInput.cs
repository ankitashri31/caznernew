using System;
using Abp.Notifications;
using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}