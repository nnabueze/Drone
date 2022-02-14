using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("drone service started");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

                Console.WriteLine("drone service failed....");

                Console.WriteLine(ex);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
