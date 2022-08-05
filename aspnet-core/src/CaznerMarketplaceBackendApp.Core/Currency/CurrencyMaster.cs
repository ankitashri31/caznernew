using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Currency
{
    public class CurrencyMaster : FullAuditedEntity<long>
    {
        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }

        public bool IsActive { get; set; }

        public CurrencyMaster()
        {

        }
        public CurrencyMaster(string currencyName, bool isActive, string currencyCode)
               : this()
        {
            CurrencyName = currencyName;            
            CurrencyCode = currencyCode;
            IsActive = isActive;

        }

    }
}
