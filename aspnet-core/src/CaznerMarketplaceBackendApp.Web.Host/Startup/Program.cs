using System.IO;
using Microsoft.AspNetCore.Hosting;
using CaznerMarketplaceBackendApp.Web.Helpers;

namespace CaznerMarketplaceBackendApp.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(opt =>
                {
                    opt.AddServerHeader = false;
                    opt.Limits.MaxRequestLineSize = 16 * 1024;
                    // opt.Limits.MaxRequestBodySize = 52428800; //50MB
                    opt.Limits.MaxRequestBodySize = 104857600;//null
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>();
        }
    }
}
