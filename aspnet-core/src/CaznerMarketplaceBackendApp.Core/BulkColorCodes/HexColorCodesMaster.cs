using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.BulkColorCodes
{
    public class HexColorCodesMaster : FullAuditedEntity<long>
    {
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string ColorFamily { get; set; }
        public bool IsActive { get; set; }

    }
}
