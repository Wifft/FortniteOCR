using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using FortniteOCR.ConsoleFormatters;
using FortniteOCR.Consumers;
using FortniteOCR.Helpers;
using FortniteOCR.Interfaces;
using FortniteOCR.Services;
using Windows.UI.WebUI;

namespace FortniteOCR
{
    public static partial class FortniteOCR
    {
        public static bool debugMode = false;

        static void Main(string[] args)
        {
            if (args.Length.Equals(1) && args[0] == "--debug") debugMode = true;  

            InstallationHelper.CheckTimebomb();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService(
                    options =>
                    {
                        options.ServiceName = "FortniteOCR by Wifft";
                    }
                )
                .ConfigureLogging(
                    (hostingContex, logging) =>
                    {
                        logging.SetMinimumLevel(LogLevel.Debug)
                            .AddFilter("Microsoft", LogLevel.Warning)
                            .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning)
                            .AddConsole(options => options.FormatterName = "wifftFormater")
                            .AddConsoleFormatter<CustomFormatter, CustomFormatter.CustomOptions>(options => options.CustomPrefix = "[FortniteOCR by Wifft] ");
                    }
                )
                .ConfigureServices(
                    services =>
                    {
                        services.AddHostedService<ConsumeOcrService>();
                        services.AddScoped<IScopedProcessingService, OcrService>();
                        services.AddSingleton<OcrService>();
                    }
                );
        }
    }
}
