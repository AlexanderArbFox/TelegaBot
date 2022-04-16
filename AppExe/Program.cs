using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz.Spi;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppExe
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .WriteTo.File(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"logging.txt")
                         .CreateLogger();
            try
            {
                IHostBuilder builder = new HostBuilder()
                                       .ConfigureAppConfiguration(confBuilder =>
                                                                   {
                                                                       confBuilder.AddJsonFile("config.json");
                                                                       confBuilder.AddCommandLine(args);
                                                                   })
                                       .ConfigureLogging((confLogging, logging) =>
                                                                   {
                                                                       logging.AddConsole();
                                                                       logging.AddDebug();
                                                                       //logging.Addserilog();
                                                                   })
                                       .ConfigureServices(services =>
                                       {
                                           //services.AddSingleton<IJobFactory, JobFactory>;
                                       });



                //await builder.RunService();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host builder error");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
