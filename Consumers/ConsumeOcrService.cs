using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using FortniteOCR.Interfaces;
using FortniteOCR.Services;

namespace FortniteOCR.Consumers
{
    internal sealed class ConsumeOcrService : BackgroundService
    {
        private readonly ILogger<OcrService> _logger;

        public IServiceProvider Services { get; }

        public ConsumeOcrService(IServiceProvider services, ILogger<OcrService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("\u001b[1m\u001b[37mFortniteOCR service started.\u001b[1m\u001b[37");
            _logger.LogInformation("\u001b[1m\u001b[37mPress \u001b[33mCTRL+C \u001b[1m\u001b[37mto stop it.\u001b[1m\u001b[37");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using IServiceScope scope = Services.CreateScope();
            IScopedProcessingService service = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

            await service.DoWork(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("\u001b[1m\u001b[37FortniteOCR service stopping...\u001b[1m\u001b[37");

            for (int i = 0; i <= 1; i++) File.Delete(Environment.GetEnvironmentVariable("tmp") + $"\\fortnite_ocr_game_capture_{i}.jpg");

            await base.StopAsync(stoppingToken);
        }
    }
}
