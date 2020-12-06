using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore;
// This method gets the service collection that SeedData.InitializeAsync()
//needs and then runs the method to seed the database. 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Original
            //CreateHostBuilder(args).Build().Run();

            // var host = BuildWebHost(args);
            // InitializeDatabase(host);
            // host.Run();

            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                SeedData.InitializeAsync(scope.ServiceProvider).Wait();
               
                //await SeedData.InitializeAsync(scope.ServiceProvider);
                
                // var services = scope.ServiceProvider;
                // var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            }
            host.Run();
        }

        // Orignal
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        // ADDITIONAL
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        
        private static void InitializeDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    SeedData.InitializeAsync(services).Wait();
                    Console.WriteLine("1111111111");
                    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    Console.WriteLine("2222222222");
                }
                catch (Exception ex)
                {
                    var logger = services
                    .GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error occurred seeding the DB.");
                }
            }
        }



    }
}
