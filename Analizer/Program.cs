using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Analizer
{
    public class Program
    {
        public static Timer Timer { get; set; }
        public static List<Analizer> Analizers { get; set; }
        public static void Main(string[] args)
        {
            Timer = new Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            Analizers = new List<Analizer>()
            {
                new Analizer()
                {
                    Name = "Ledetect",
                    AvailableLaboratoryServicesCode = new List<string>()
                    {
                        "229", "258", "311", "323", "346", "415", "501", "543", "557", "619", "659"
                    },
                    IsBusy = false,
                    Progress = 0
                },
                new Analizer()
                {
                    Name = "Biorad",
                    AvailableLaboratoryServicesCode = new List<string>()
                    {
                        "176", "258", "287", "543", "548", "619", "659", "797", "836", "855"
                    },
                    IsBusy = false,
                    Progress = 0
                }
            };
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static Task HandleTimer(string name)
        {
            return Task.Run(() =>
            {
                Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress += 10;
                if (Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress == 100)
                {
                    Timer.Stop();
                    Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress = 0;
                    Analizers.FirstOrDefault(p => p.Name.Equals(name)).IsBusy = false;
                }
            });
        }
    }
}
