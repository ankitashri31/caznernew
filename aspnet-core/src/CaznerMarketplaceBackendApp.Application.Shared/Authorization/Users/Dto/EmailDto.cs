using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Users.Dto
{
    public class EmailDto
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string EmailTo { get; set; }
    }
}
