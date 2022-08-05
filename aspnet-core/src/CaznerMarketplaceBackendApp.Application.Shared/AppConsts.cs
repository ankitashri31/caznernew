using System;
using System.ComponentModel;

namespace CaznerMarketplaceBackendApp
{
    /// <summary>
    /// Some consts used in the application.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";

        public const int ResizedMaxProfilPictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilPictureBytesUserFriendlyValue = 5;

        public const string TokenValidityKey = "token_validity_key";
        public const string RefreshTokenValidityKey = "refresh_token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";

        public const string TokenType = "token_type";

        public static string UserIdentifier = "user_identifier";

        public const string ThemeDefault = "default";
        public const string Theme2 = "theme2";
        public const string Theme3 = "theme3";
        public const string Theme4 = "theme4";
        public const string Theme5 = "theme5";
        public const string Theme6 = "theme6";
        public const string Theme7 = "theme7";
        public const string Theme8 = "theme8";
        public const string Theme9 = "theme9";
        public const string Theme10 = "theme10";
        public const string Theme11 = "theme11";

        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);

        public const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";

        public enum SubscriptionUserType
        {
            Supplier = 1,
            Distributor = 2
        }
        public enum SubscriptionCurrencyType
        {
            AUD = 1,
            Doller = 2
        }
        public enum SubscriptionBillingType
        {
            Monthly = 1,
            Yearly = 2
        }
        public enum SubscriptionSymbolType
        {
            [Description("$")]
            Doller = 1
        }
        public enum ProductOptionEnum
        {
            Colour = 1,
            Material = 2,
            Size = 3,
            Style = 4
        }
        public enum ProductMediaTypes
        {
            LineArt = 1,
            LifeStyleImages = 2,
            OtherMedia = 3
        }

        public enum ProductChargeTax
        {
            IsChargeTax = 1,
            IsProductHasPriceVariant = 2
        }

        public enum EDataType
        {
            MainHeading = 1,
            SubHeading = 2,
            TableHeader = 3,
            TableRow = 4
        }

        public enum ProductSortByEnum
        {
            All = 1,
            Relevance = 2,
            Name = 3,
            Price = 4,
            Manufacture = 5,
            PrintMethod = 6,
            Size = 7,
            Colour = 8
        }
        public enum FilterByEnum
        {
           ascending=1,
           Descending=2
        }

        public enum ProductType
        {
            Compartment = 1,
            General = 2
        }

        public enum OptionTypes
        {
            Size = 1,
            Color = 2,
            Material = 3,
            Style = 4
        }

        public enum LogoBannerType
        {
            Logo = 1,
            Banner = 2
        }
        public enum ColorSelectionType
        {
            Color = 1,
            Stiches = 2
        }
    }
}
