using Abp;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Importing.Dto
{
    public class ImportProductFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public string BackroundJobId { get; set; }
        public UserIdentifier User { get; set; }

        public long StatusId { get; set; }
        public List<int> TenantIds { get; set; }

        public int TenantIdForDatabase { get; set; }

        public int ExpectedExcelRowsCount { get; set; }
        public PerformContext Context { get; set; }

    }
}
