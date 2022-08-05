using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductNotesDto : EntityDto<long>
    {
        public long ProductId { get; set; }
        public string NoteDescription { get; set; }
    }
}
