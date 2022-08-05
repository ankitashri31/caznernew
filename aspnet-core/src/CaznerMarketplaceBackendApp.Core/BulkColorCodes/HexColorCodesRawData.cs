using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.BulkColorCodes
{
    public class HexColorCodesRawData
    {
        [Key]
        public long Id { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string ColorFamily { get; set; }

    }
}
