using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;

namespace CaznerMarketplaceBackendApp.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsReadDone { get; set; }

        public virtual int? NumberOfRows { get; set; }

        [Required] public virtual byte[] Bytes { get; set; }

        public BinaryObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public BinaryObject(int? tenantId, byte[] bytes, string description = null, bool isReadDone=false, int? numberOfRows=null)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            IsReadDone = isReadDone;
            NumberOfRows = numberOfRows;
        }
    }
}