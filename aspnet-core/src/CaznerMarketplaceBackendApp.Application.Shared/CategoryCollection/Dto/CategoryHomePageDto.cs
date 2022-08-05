using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CategoryHomePageDto
    {
        public long Id { get; set; }
        public int TenantId { get; set; }

        public List<long> CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string EncryptedTenantId { get; set; }
        public int SequenceNumber { get; set; }

        public long AssignmentId { get; set; }
    }
}
