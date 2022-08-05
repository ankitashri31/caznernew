using Abp.MultiTenancy;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaznerMarketplaceBackendApp.Migrations.Seed.Host
{
    class DefaultCountryCreator
    {
        public static List<Countries> InitialCountries => GetInitialCountries();

        private readonly CaznerMarketplaceBackendAppDbContext _context;

        private static List<Countries> GetInitialCountries()
        {
            var tenantId = CaznerMarketplaceBackendAppConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<Countries>
            {
                new Countries("Afghanistan",true,null,"AFG",6,"Low"),
                new Countries("Albania",true,null,"ALB",4,"Upper‐middle"),
                new Countries("Algeria",true,null,"DZA",1,"Upper‐middle"),
                new Countries("Andorra",true,null,"AND",4,"High"),
                new Countries("Angola",true,null,"AGO",1,"Lower‐middle"),
                new Countries("Antigua and Barbuda",true,null,"ATG",5,"High"),
                new Countries("Argentina",true,null,"ARG",5,"Upper‐middle"),
                new Countries("Armenia",true,null,"ARM",4,"Upper‐middle"),
                new Countries("Austria",true,null,"AUT",4,"High"),
                new Countries("Australia",true,null,"AUS",2,"High"),
                new Countries("Azerbaijan",true,null,"AZE",4,"Upper‐middle"),

                new Countries("Bahamas",true,null,"BHS",5,"High"),
                new Countries("Bahrain",true,null,"BHR",6,"High"),
                new Countries("Bangladesh",true,null,"BGD",3,"Lower‐middle"),
                new Countries("Barbados",true,null,"BRB",5,"High"),
                new Countries("Belarus",true,null,"BLR",4,"Upper‐middle"),
                new Countries("Belgium",true,null,"BEL",4,"High"),
                new Countries("Belize",true,null,"BLZ",5,"Upper‐middle"),
                new Countries("Benin",true,null,"BEN",1,"Low"),
                new Countries("Bhutan",true,null,"BTN",3,"Lower‐middle"),
                new Countries("Bolivia (Plurinational State of)",true,null,"BOL",5,"Lower‐middle"),
                new Countries("Bosnia and Herzegovina",true,null,"BIH",4,"Lower‐middle"),
                new Countries("Botswana",true,null,"BWA",1,"Upper‐middle"),
                new Countries("Brazil",true,null,"BRA",5,"Upper‐middle"),
                new Countries("Brunei Darussalam",true,null,"BRN",2,"High"),
                new Countries("Bulgaria",true,null,"BGR",4,"Upper‐middle"),
                new Countries("Burkina Faso",true,null,"BFA",1,"Low"),
                new Countries("Burundi",true,null,"BDI",1,"Low"),

                new Countries("Cabo Verde",true,null,"CPV",1,"Lower‐middle"),
                 new Countries("Cambodia",true,null,"KHM",2,"Lower‐middle"),
                new Countries("Cameroon",true,null,"CMR",1,"Lower‐middle"),
                new Countries("Canada",true,null,"CAN",5,"High"),
                new Countries("Central African Republic",true,null,"CAF",1,"Low"),
                new Countries("Chad",true,null,"TCD",1,"High"),
                new Countries("Chile",true,null,"CHL",5,"High"),
                new Countries("China",true,null,"CHN",2,"Upper‐middle"),
                new Countries("Colombia",true,null,"COL",5,"Lower‐middle"),
                new Countries("Comoros",true,null,"COM",1,"Lower‐middle"),
                new Countries("Congo",true,null,"COG",1,"Lower‐middle"),
                new Countries("Cook Islands",true,null,"COK",2,"High"),
                new Countries("Costa Rica",true,null,"CRI",5,"Upper‐middle"),
                new Countries("Côte d'Ivoire",true,null,"CIV",1,"Lower‐middle"),
                new Countries("Croatia",true,null,"HRV",4,"High"),
                new Countries("Cuba",true,null,"CUB",5,"Upper‐middle"),
                new Countries("Cyprus",true,null,"CYP",4,"High"),
                new Countries("Czech Republic",true,null,"CZE",4,"High"),
                new Countries("Democratic People's Republic of Korea",true,null,"PRK",3,"Low"),
                new Countries("Democratic Republic of the Congo",true,null,"COD",1,"Low"),
                new Countries("Denmark",true,null,"DNK",4,"High"),
                new Countries("Djibouti",true,null,"DJI",6,"Lower‐middle"),
                new Countries("Dominica",true,null,"DMA",5,"Upper‐middle"),
                new Countries("Dominican Republic",true,null,"DOM",5,"Upper‐middle"),
                new Countries("Ecuador",true,null,"ECU",5,"Upper‐middle"),
                new Countries("Egypt",true,null,"EGY",6,"Lower‐middle"),
                new Countries("El Salvador",true,null,"SLV",5,"Lower‐middle"),
                new Countries("Equatorial Guinea",true,null,"GNQ",1,"Upper‐middle"),
                new Countries("Eritrea",true,null,"ERI",1,"Low"),
                new Countries("Estonia",true,null,"EST",4,"High"),
                new Countries("Eswatini",true,null,"SWZ",1,"Lower‐middle"),
                new Countries("Ethiopia",true,null,"ETH",1,"Lower‐middle"),
                new Countries("Fiji",true,null,"FJI",2,"Upper‐middle"),
                new Countries("Finland",true,null,"FIN",4,"High"),
                new Countries("France",true,null,"FRA",4,"High"),
                 new Countries("Gabon",true,null,"GAB",1,"Upper‐middle"),
                 new Countries("Gambia",true,null,"GMB",1,"Low"),
                 new Countries("Georgia",true,null,"GEO",4,"Upper‐middle"),
                 new Countries("Germany",true,null,"DEU",4,"High"),
                new Countries("Ghana",true,null,"GHA",1,"Lower‐middle"),
                new Countries("Greece",true,null,"GRC",4,"High"),
                new Countries("Grenada",true,null,"GRD",5,"Upper‐middle"),
                 new Countries("Guatemala",true,null,"GTM",5,"Upper‐middle"),
                new Countries("Guinea",true,null,"GIN",1,"Low"),
                new Countries("Guinea-Bissau",true,null,"GNB",1,"Low"),
                new Countries("Guyana",true,null,"GUY",5,"Upper‐middle"),
                new Countries("Haiti",true,null,"HTI",5,"Low"),
                new Countries("Honduras",true,null,"HND",5,"Lower‐middle"),
                new Countries("Hungary",true,null,"HUN",4,"High"),
                new Countries("Iceland",true,null,"ISL",4,"High"),
                new Countries("India",true,null,"IND",3,"Lower‐middle"),
                new Countries("Indonesia",true,null,"IDN",3,"Lower‐middle"),
                new Countries("Iran (Islamic Republic of)",true,null,"IRN",6,"Lower‐middle"),
                new Countries("Iraq",true,null,"IRQ",6,"Upper‐middle"),
                new Countries("Ireland",true,null,"IRL",4,"High"),
                new Countries("Israel",true,null,"ISR",4,"High"),
                new Countries("Italy",true,null,"ITA",4,"High"),
                new Countries("Jamaica",true,null,"JAM",5,"Upper‐middle"),
                new Countries("Japan",true,null,"JPN",2,"High"),
                new Countries("Jordan",true,null,"JOR",6,"Upper‐middle"),
                new Countries("Kazakhstan",true,null,"KAZ",3,"Upper‐middle"),
                new Countries("Kenya",true,null,"KEN",1,"Lower‐middle"),
                new Countries("Kiribati",true,null,"KIR",2,"Lower‐middle"),
                new Countries("Kuwait",true,null,"KWT",6,"High"),
                new Countries("Kyrgyzstan",true,null,"KGZ",4,"Lower‐middle"),
                new Countries("Lao People's Democratic Republic",true,null,"LAO",2,"Lower‐middle"),
                new Countries("Latvia",true,null,"LVA",4,"High"),
                new Countries("Lebanon",true,null,"LBN",6,"Upper‐middle"),
                new Countries("Lesotho",true,null,"LSO",1,"Lower‐middle"),
                new Countries("Liberia",true,null,"LBR",1,"Low"),
                new Countries("Libya",true,null,"LBY",6,"Upper‐middle"),
                new Countries("Lithuania",true,null,"LTU",4,"High"),
                new Countries("Luxembourg",true,null,"LUX",4,"High"),
                new Countries("Madagascar",true,null,"MDG",1,"Low"),
                new Countries("Malawi",true,null,"MWI",1,"Low"),
                new Countries("Malaysia",true,null,"MYS",2),
                new Countries("Maldives",true,null,"MDV",3,"Upper‐middle"),
                 new Countries("Mali",true,null,"MLI",1,"Low"),
                 new Countries("Malta",true,null,"MLT",4,"High"),

                new Countries("Marshall Islands",true,null,"MHL",2,"Upper‐middle"),
                new Countries("Mauritania",true,null,"MRT",1,"Lower‐middle"),
                new Countries("Mauritius",true,null,"MUS",1,"Upper‐middle"),
                new Countries("Mexico",true,null,"MEX",5,"Upper‐middle"),
                new Countries("Micronesia (Federated States of)",true,null,"FSM",2,"Lower‐middle"),
                new Countries("Monaco",true,null,"MCO",4,"High"),
                new Countries("Mongolia",true,null,"MNG",2,"Lower‐middle"),
                new Countries("Montenegro",true,null,"MNE",4,"Upper‐middle"),
                new Countries("Morocco",true,null,"MAR",6,"Lower‐middle"),
                new Countries("Mozambique",true,null,"MOZ",1,"Low"),
                new Countries("Myanmar",true,null,"MMR",3,"Lower‐middle"),

                new Countries("Namibia",true,null,"NAM",1,"Upper‐middle"),
                 new Countries("Nauru",true,null,"NRU",2,"Upper‐middle"),
                 new Countries("Nepal",true,null,"NPL",3,"Low"),
                 new Countries("Netherlands",true,null,"NLD",4,"High"),
                  new Countries("New Zealand",true,null,"NZL",2,"High"),
                  new Countries("Nicaragua",true,null,"NIC",5,"Lower‐middle"),
                new Countries("Niger",true,null,"NER",1,"Low"),
                new Countries("Nigeria",true,null,"NGA",1,""),
                 new Countries("Niue",true,null,"NIU",2,"High"),
                new Countries("Norway",true,null,"NOR",4,"High"),            
                
                new Countries("Oman",true,null,"OMN",6,"High"),
                new Countries("Pakistan",true,null,"PAK",6,"Lower‐middle"),
                new Countries("Palau",true,null,"PLW",2,"High"),
                new Countries("Panama",true,null,"PAN",5,"High"),
                new Countries("Papua New Guinea",true,null,"PNG",2,"Lower‐middle"),
                new Countries("Paraguay",true,null,"PRY",5,"Upper‐middle"),
                new Countries("Peru",true,null,"PER",2,"Upper‐middle"),
                new Countries("Philippines",true,null,"PHL",2,"Lower‐middle"),
                new Countries("Poland",true,null,"POL",4,"High"),
                new Countries("Portugal",true,null,"PRT",4,"High"),

                new Countries("Qatar",true,null,"QAT",6,"High"),
                new Countries("Romania",true,null,"ROU",4,"Upper‐middle"),
                new Countries("Republic of Korea",true,null,"KOR",2,"High"),
                new Countries("Russian Federation",true,null,"RUS",4,"Upper‐middle"),
                new Countries("Rwanda",true,null,"RWA",1,"Low"),
               
               new Countries("Saint Kitts and Nevis",true,null,"KNA",5,"High"),
               new Countries("Saint Lucia",true,null,"LCA",5,"Upper‐middle"),
               new Countries("Saint Vincent and the Grenadines",true,null,"VCT",5,"Upper‐middle"),
               new Countries("Samoa",true,null,"WSM",2,"Upper‐middle"),
               new Countries("San Marino",true,null,"SMR",4,"High"),
               new Countries("Sao Tome and Principe",true,null,"STP",1,"Lower‐middle"),
               new Countries("Saudi Arabia",true,null,"SAU",6,"High"),
               new Countries("Senegal",true,null,"SEN",1,"Lower‐middle"),
               new Countries("Serbia",true,null,"SRB",4,"Upper‐middle"),
               new Countries("Seychelles",true,null,"SYC",1,"High"),
               new Countries("Sierra Leone",true,null,"SLE",1,"Low"),
               new Countries("Singapore",true,null,"SGP",2,"High"),
               new Countries("Slovakia",true,null,"SVK",4,"High"),
               new Countries("Slovenia",true,null,"SVN",4,"High"),
                new Countries("Solomon Islands",true,null,"SLB",2,"Lower‐middle"),
                new Countries("Somalia",true,null,"SOM",6,"Low"),
                new Countries("South Africa",true,null,"ZAF",1,"Upper‐middle"),
                 new Countries("South Sudan",true,null,"SSD",1,"Low"),
                 new Countries("Spain",true,null,"ESP",4,"High"),
                  new Countries("Sri Lanka",true,null,"LKA",3,"Upper‐middle"),
                new Countries("Sudan",true,null,"SDN",6,"Lower‐middle"),
                new Countries("Suriname",true,null,"SUR",5,"Upper‐middle"),
                new Countries("Sweden",true,null,"SWE",4,"High"),
                new Countries("Switzerland",true,null,"CHE",4,"High"),
                new Countries("Syria",true,null,"SYR",6,"Low"),

                new Countries("Tajikistan",true,null,"TJK",4,"Low"),
                new Countries("Thailand",true,null,"THA",3,"Upper‐middle"),
                new Countries("The Republic of North Macedonia",true,null,"MKD",1,"Upper‐middle"),
                new Countries("Timor-Leste",true,null,"TLS",3,"Lower‐middle"),
                new Countries("Togo",true,null,"TGO",1,"Low"),
                new Countries("Tonga",true,null,"TON",2,"Upper‐middle"),
                new Countries("Trinidad and Tobago",true,null,"TTO",5,"High"),
                new Countries("Tunisia",true,null,"TUN",6,"Lower‐middle"),
                new Countries("Turkey",true,null,"TUR",4,"Upper‐middle"),
                new Countries("Turkmenistan",true,null,"TKM",4,"Upper‐middle"),
                new Countries("Tuvalu",true,null,"TUV",2,"Upper‐middle"),

                new Countries("Uganda",true,null,"UGA",1,"Low"),
                new Countries("United Republic of Tanzania",true,null,"TZA",1,"Low"),
                new Countries("Ukraine",true,null,"UKR",4,"Lower‐middle"),
                 new Countries("United Arab Emirates",true,null,"ARE",6,"High"),
                  new Countries("United Kingdom",true,null,"GBR",4,"High"),
                new Countries("United States of America",true,null,"USA",5,"High"),
                new Countries("Uruguay",true,null,"URY",5,"High"),
                new Countries("Uzbekistan",true,null,"UZB",4,"Lower‐middle"),
                 new Countries("Vanuatu",true,null,"VUT",2,"Lower‐middle"),
                new Countries("Venezuela (Bolivarian Republic of)",true,null,"VEN",5,"Upper‐middle"),
                new Countries("Viet Nam",true,null,"VNM",2,"Lower‐middle"),
                new Countries("Yemen",true,null,"YEM",6,"Low"),
                new Countries("Zambia",true,null,"ZMB",1,"Lower‐middle"),
                new Countries("Zimbabwe",true,null,"ZWE",1,"Lower‐middle")
            };
        }
        public DefaultCountryCreator(CaznerMarketplaceBackendAppDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateCountries();
        }

        private void CreateCountries()
        {
            foreach (var contry in InitialCountries)
            {
                AddCountryIfNotExists(contry);
            }
        }

        private void AddCountryIfNotExists(Countries countries)
        {
            if (_context.Countries.IgnoreQueryFilters().Any(l => l.CountryName == countries.CountryName && l.IsActive == countries.IsActive
                                          && l.ISO3Code == countries.ISO3Code && l.RegionId== countries.RegionId
                                          && l.WorldBankIncomeGroup == countries.WorldBankIncomeGroup))
            {
                return;
            }

            _context.Countries.Add(countries);
            _context.SaveChanges();
        }
    }
}
