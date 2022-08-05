using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class BrandingAdditionalModel
    {
        public double Quantity { get; set; }
        // public BrandingAdditionalQtyDto QuantityObj { get; set; }
        public BrandingAdditionalPriceDto[] PricesObj { get; set; }

    }

    public class BrandingAdditionalQtyDto
    {
        public long Id { get; set; }
        public double Quantity { get; set; }
        public long AssignmentQtyId { get; set; }
    }

    public class BrandingAdditionalPriceDto
    {
        public long Id { get; set; }
        public double Price { get; set; }
        public long AssignmentPriceId { get; set; }
    }
}
