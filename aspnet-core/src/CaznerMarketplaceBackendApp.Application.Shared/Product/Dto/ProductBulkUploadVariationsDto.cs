using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductBulkUploadVariationsDto : EntityDto<long>
    {            
        public long ProductId { get; set; }
        public int productOptionId { get; set; }

        public List<OptionValue> ProductOptionValue { get; set; }
        //public string[] ProductOptionValue { get; set; }
        public bool IsActive { get; set; }
    }

    public class OptionValue 
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    }
