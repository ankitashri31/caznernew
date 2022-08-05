using Abp.MultiTenancy;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaznerMarketplaceBackendApp.Migrations.Seed.Host
{
    class DefaultCurrencyCreator
    {
        public static List<CurrencyMaster> InitialCurrencyMaster => GetInitialCurrecy();

        private readonly CaznerMarketplaceBackendAppDbContext _context;
        private static List<CurrencyMaster> GetInitialCurrecy()
        {
            var tenantId = CaznerMarketplaceBackendAppConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<CurrencyMaster>
            {
               new CurrencyMaster("Afghanistan Afghani",true,"AFN"),
               new CurrencyMaster("Albanian Lek",true,"ALL"),
               new CurrencyMaster("Algerian Dinar",true,"DZD"),
                new CurrencyMaster("Angolan Kwanza",true,"AON"),
               new CurrencyMaster("Euro",true,"EUR"),
              new CurrencyMaster("Argentine Peso",true,"ARS"),
              new CurrencyMaster("Australian Dollar",true,"AUD"),
               new CurrencyMaster("Bahamian Dollar",true,"BSD"),
               new CurrencyMaster("Baharaini Dinar",true,"BHD"),
               new CurrencyMaster("Balboa",true,"PAB"),
               new CurrencyMaster("Barbados Dollar",true,"BBD"),
                new CurrencyMaster("Belarusian Ruble",true,"BYN"),
                 new CurrencyMaster("Belize Dollar",true,"BZD"),
                 new CurrencyMaster("Bhat",true,"THB"),
                  new CurrencyMaster("Bolivar Fuerte",true,"VEF"),
                  new CurrencyMaster("Boliviano",true,"BOB"),
                  new CurrencyMaster("Botswana Pula",true,"BWP"),
                  new CurrencyMaster("Brazilian Real",true,"BRL"),
                  new CurrencyMaster("Brunei Dollar",true,"BND"),
               new CurrencyMaster("Bulgarian Lev",true,"BGL"),
                new CurrencyMaster("Burundi Franc",true,"BIF"),
                 new CurrencyMaster("Canadian Dollar",true,"CAD"),
               new CurrencyMaster("Cape Verde Escudo",true,"CVE"),
               new CurrencyMaster("CFA Franc",true,"XOF"),
                  new CurrencyMaster("CFA Franc",true,"XAF"),
                  new CurrencyMaster("Chilean Peso",true,"CLP"),
                    new CurrencyMaster("Colombian Peso",true,"COP"),
                    new CurrencyMaster("Comoros Franc",true,"KMF"),
                    new CurrencyMaster("Congolese Franc",true,"CDF"),
                     new CurrencyMaster("Convertible Mark",true,"BAM"),
                     new CurrencyMaster("Costa Rican Colone",true,"CRC"),
                     new CurrencyMaster("Cuban Peso",true,"CUP"),
                     new CurrencyMaster("Danish Krone",true,"DKK"),
                     new CurrencyMaster("Denar",true,"MKD"),
                     new CurrencyMaster("Djibouti Franc",true,"DJF"),
                      new CurrencyMaster("Dominican Peso",true,"DOP"),
                      new CurrencyMaster("Dong",true,"VND"),
                      new CurrencyMaster("Dram",true,"AMD"),
               new CurrencyMaster("Eastern Caribbean Dollar",true,"XCD"),
                new CurrencyMaster("Egyptian Pound",true,"EGP"),
                 new CurrencyMaster("Ethiopian Birr",true,"ETB"),
                 new CurrencyMaster("Euro",true,"EUR"),
                 new CurrencyMaster("Fiji Dollar",true,"FJD"),
                 new CurrencyMaster("Gambian Dalasi",true,"GMD"),
                 new CurrencyMaster("Ghana Cedi",true,"GHC"),
                 new CurrencyMaster("Guarani",true,"PYG"),
                 new CurrencyMaster("Guatemalan Quetzal",true,"GTQ"),
                 new CurrencyMaster("Guinea Franc",true,"GNF"),
                 new CurrencyMaster("Guyana Dollar",true,"GYD"),
                 new CurrencyMaster("Haitian Gourde",true,"HTG"),
                 new CurrencyMaster("Honduran Lempira",true,"HNL"),
                 new CurrencyMaster("Hryvnia",true,"UAH"),
                 new CurrencyMaster("Hungarian Forint",true,"HUF"),

                new CurrencyMaster("Icelandic Krona",true,"ISK"),
                new CurrencyMaster("Indian Rupee",true,"INR"),
                new CurrencyMaster("Indonesian Rupiah",true,"IDR"),
                new CurrencyMaster("Iranian Rial",true,"IRR"),
                new CurrencyMaster("Iraqi Dinar",true,"IQD"),
                new CurrencyMaster("Jamaican Dollar",true,"JMD"),
                new CurrencyMaster("Japanese Yen",true,"JPY"),
                new CurrencyMaster("Jordanian Dinar",true,"JOD"),
                 new CurrencyMaster("Kenyan Shilling",true,"KES"),
                  new CurrencyMaster("Kina",true,"PGK"),
                  new CurrencyMaster("Kip",true,"LAK"),
                   new CurrencyMaster("Kuna",true,"HRK"),
                   new CurrencyMaster("Kuwaiti Dinar",true,"KWD"),
                   new CurrencyMaster("Kwacha",true,"ZMK"),                  
                   new CurrencyMaster("Kyat",true,"MMK"),
                   new CurrencyMaster("Lari",true,"GEL"),
                   new CurrencyMaster("Lebanese Pound",true,"LBP"),
                   new CurrencyMaster("Leone ",true,"SLL"),
                    new CurrencyMaster("Lesotho Loti",true,"LSL"),
                    new CurrencyMaster("Liberian Dollar",true,"LRD"),
                     new CurrencyMaster("Libyan Dinar",true,"LYD"),
                     new CurrencyMaster("Lilangeni",true,"SZL"),
                      new CurrencyMaster("Malagasy Ariary",true,"MGA"),
                      new CurrencyMaster("Malawian Kwacha",true,"MWK"),
                    new CurrencyMaster("Manat",true,"AZN"),
                    new CurrencyMaster("Mauritius Rupee",true,"MUR"),
                     new CurrencyMaster("Mexican Peso",true,"MXN"),
                      new CurrencyMaster("Moldova Leu",true,"MDL"),
                      new CurrencyMaster("Moroccan Dirham",true,"MAD"),
                      new CurrencyMaster("Mozambican Metical",true,"MZM"),
                      new CurrencyMaster("Naira",true,"NGN"),
                      new CurrencyMaster("Nakfa",true,"ERN"),
                       new CurrencyMaster("Namibian Dollar",true,"NAD"),
                       new CurrencyMaster("Nepalese Rupee",true,"NPR"),
                        new CurrencyMaster("New Dobra",true,"STN"),
                        new CurrencyMaster("New Manat",true,"TMT"),
                        new CurrencyMaster("New Ouguiya",true,"MRU"),
                        new CurrencyMaster("New Sheqel",true,"ILS"),
                        new CurrencyMaster("New Zealand Dollar",true,"NZD"),
                        new CurrencyMaster("Ngultrum",true,"BTN"),
                        new CurrencyMaster("Nicaraguan Cordoba Oro",true,"NIO"),
                         new CurrencyMaster("North Korean Won",true,"KPW"),
                          new CurrencyMaster("Norway Krone",true,"NOK"),
                          new CurrencyMaster("Nuevo Sol",true,"PEN"),
                           new CurrencyMaster("Omani Rial",true,"OMR"),
                           new CurrencyMaster("Pa'anga",true,"TOP"),
                            new CurrencyMaster("Pakistani Rupee",true,"PKR"),
                            new CurrencyMaster("Philippines Peso",true,"PHP"),
                            new CurrencyMaster("Pound Sterling",true,"GBP"),
                            new CurrencyMaster("Qatar Rial",true,"QAR"),
                            new CurrencyMaster("Rand",true,"ZAR"),
                             new CurrencyMaster("Riel",true,"KHR"),
                             new CurrencyMaster("Ringgit",true,"MYR"),
                              new CurrencyMaster("Romania Leu",true,"RON"),
                               new CurrencyMaster("Rouble",true,"RUB"),
                               new CurrencyMaster("Rufiyaa",true,"MVR"),
                               new CurrencyMaster("Rwandan Franc",true,"RWF"),
                                new CurrencyMaster("Saudi Riyal",true,"SAR"),
                                 new CurrencyMaster("Serbian Dinar",true,"RSD"),
                               new CurrencyMaster("Seychelles Rupee ",true,"SCR"),
                            new CurrencyMaster("Singapore Dollar",true,"SGD"),
                             new CurrencyMaster("Solomon Dollar",true,"SBD"),
                             new CurrencyMaster("Som",true,"KGS"),
                             new CurrencyMaster("Somalian Shilling",true,"SOS"),
                             new CurrencyMaster("Somoni",true,"TJS"),
                              new CurrencyMaster("South Sudanese Pound",true,"SSP"),
                              new CurrencyMaster("Sri Lanka Rupee",true,"LKR"),
                              new CurrencyMaster("Sudanese Pound",true,"SDG"),
                              new CurrencyMaster("Sum",true,"UZS"),
                                new CurrencyMaster("Surinamese Dollar",true,"SRD"),
                            new CurrencyMaster("Swedish Krona",true,"SEK"),
                            new CurrencyMaster("Swiss Franc",true,"CHF"),
                            new CurrencyMaster("Syrian Pound",true,"SYP"),
                             new CurrencyMaster("Taka",true,"BDT"),
                              new CurrencyMaster("Tala",true,"WST"),
                               new CurrencyMaster("Tanzanian Shilling",true,"TZS"),
                                 new CurrencyMaster("Tenge",true,"KZT"),
                                 new CurrencyMaster("Trinidad and Tobago Dollar",true,"TTD"),
                                 new CurrencyMaster("Tugrik",true,"MNT"),
                                  new CurrencyMaster("Tunisian Dinar",true,"TND"),
                              new CurrencyMaster("Turkish Lira",true,"TRY"),
                              new CurrencyMaster("U.A.Emirates Dirham",true,"AED"),
                              new CurrencyMaster("Ugandan shilling",true,"UGS"),
                               new CurrencyMaster("Uruguayan Peso",true,"UYU"),
                                new CurrencyMaster("US Dollar",true,"USD"),
                                 new CurrencyMaster("US Dollar",true,"ZWD"),
                                 new CurrencyMaster("Vatu",true,"VUV"),
                                 new CurrencyMaster("Won",true,"KRW"),
                                 new CurrencyMaster("Yemeni Rial",true,"YER"),
                                 new CurrencyMaster("Yuan Renminbi",true,"CNY"),
                                  new CurrencyMaster("Zloty",true,"PLN"),

            };
        }
        public DefaultCurrencyCreator(CaznerMarketplaceBackendAppDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateCurrency();
        }
        private void CreateCurrency()
        {
            foreach (var currency in InitialCurrencyMaster)
            {
                AddCurrencyIfNotExists(currency);
            }
        }
        private void AddCurrencyIfNotExists(CurrencyMaster currency)
        {
            if (_context.CurrencyMaster.IgnoreQueryFilters().Any(l => l.CurrencyName == currency.CurrencyName && l.IsActive == currency.IsActive && l.CurrencyCode == currency.CurrencyCode ))
            {
                return;
            }

            _context.CurrencyMaster.Add(currency);
            _context.SaveChanges();
        }

    }
}
