using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Currency.Dto
{
    public class CreateOrUpdateCurrency
    {
        public string CurrencyName { get; set; }
        public bool IsActive { get; set; }
    }
}
