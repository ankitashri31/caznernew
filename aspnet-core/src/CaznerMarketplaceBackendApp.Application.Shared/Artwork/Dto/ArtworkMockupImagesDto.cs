using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Artwork.Dto
{
   public  class ArtworkMockupImagesDto : EntityDto<long>
    {
        public string ImageName { get; set; }
        public long ArtworkId { get; set; }
        public string ImagePath { get; set; }
        public string ImageSize { get; set; }
        public byte[] ImageFileData { get; set; }
        public string ImageExtension { get; set; }
        public bool IsActive { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
