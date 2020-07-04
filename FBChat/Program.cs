using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting; 
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace FBChat
{
    public class Program
    {
        private static string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] | {Message:l}{NewLine}{Exception}";
        
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", true)
            .AddEnvironmentVariables()
            .Build();

        //public static Logger Logger { get; } = new LoggerConfiguration()
        //    .ReadFrom.Configuration(Configuration)          
        //    .WriteTo.File($"logs/fb-chat {DateTime.Now:yyyyMMdd}.log",
        //                      outputTemplate: outputTemplate,
        //                      fileSizeLimitBytes: 100_000_000,
        //                      rollOnFileSizeLimit: true)
        //    .Enrich.FromLogContext()
        //    .CreateLogger();

        public static void Main(string[] args)
        {          
           
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) => {
                loggerConfiguration.ReadFrom.Configuration(Configuration)
                .WriteTo.File($"logs/fb-chat {DateTime.Now:yyyyMMdd}.log",
                                  outputTemplate: outputTemplate,
                                  fileSizeLimitBytes: 100_000_000,
                                  rollOnFileSizeLimit: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
