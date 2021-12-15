using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Rubix.Deamon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var mt = "{LogLevel:u1}|{SourceContext}|{Message:l}|{Properties}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
              .WriteTo.File(Directory.GetCurrentDirectory()+ "/Logs/log-.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
