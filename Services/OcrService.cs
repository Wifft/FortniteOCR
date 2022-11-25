using Microsoft.Extensions.Logging;

using System.Diagnostics;

using FortniteOCR.Helpers;
using FortniteOCR.Interfaces;
using FortniteOCR.Models;

namespace FortniteOCR.Services
{
    internal sealed class OcrService : IScopedProcessingService
    {
        private static readonly GameDecodedInfo gameDecodedInfo = new();

        private readonly ILogger<OcrService> _logger;
        
        private readonly ConsoleSpinner consoleSpinner = new();

        public OcrService(ILogger<OcrService> logger)
        {
            _logger = logger;
            consoleSpinner.Delay = 300;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            Console.Write("\u001b[31m[FortniteOCR by Wifft]\u001b[1m\u001b[37m Please, enter you assigned observer ID: ");
            _ = uint.TryParse(Console.ReadLine(), out uint observerId);
            _logger.LogInformation("The current observer ID is: " + observerId);

            while (!stoppingToken.IsCancellationRequested) {
                try
                {
                    consoleSpinner.Turn(displayMsg: "\u001b[31m[FortniteOCR by Wifft]\u001b[1m\u001b[37m Looking for running Fortnite instance", sequenceCode: 4);

                    Process? gameProcess = GetGameProcess();
                    if (gameProcess != null) _logger.LogInformation("Fortnite instance found!");

                    while (gameProcess != null)
                    {
                        await OcrHelper.GetResults()
                            .ContinueWith(task => OcrHelper.ProcessResults(gameDecodedInfo, task.Result, observerId, _logger), stoppingToken);
                        gameProcess = GetGameProcess();

                        await Task.Delay(1000, stoppingToken);
                    }
                } 
                catch (Exception e)
                {
                    _logger.LogError("\u001b[1m\u001b[37mERROR -> " + e.Message + "\u001b[37m");
                }
            };
        }

        private static Process? GetGameProcess()
        {
            Process? targetProcess = null;

            Process[] processesList = Process.GetProcesses();
            foreach (Process process in processesList)
            {
                bool isProcessRunning = process.ProcessName.Equals("FortniteClient-Win64-Shipping");
                bool processHasWindow = !process.MainWindowHandle.ToString().Equals("0");
                if (isProcessRunning && processHasWindow) targetProcess = process;
            }

            return targetProcess;
        }
    }
}
