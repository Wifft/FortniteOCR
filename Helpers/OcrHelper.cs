using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

using FortniteOCR.Models;
using FortniteOCR.Services;
using FortniteOCR.Clients;

namespace FortniteOCR.Helpers
{
    internal static class OcrHelper
    {
        public static async Task<List<OcrResult>> GetResults()
        {
            Language language = new("es");
            if (!OcrEngine.IsLanguageSupported(language)) throw new Exception(language.LanguageTag + " is not supported in this system.");

            List<OcrResult> results = new();
            
            List<string> screenshots = ScreenReadingHelper.GetScreenshoots();
            foreach (string screenshot in screenshots)
            {
                FileStream fileStream = File.OpenRead(screenshot);

                BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());
                SoftwareBitmap softwareBitmap = await bitmapDecoder.GetSoftwareBitmapAsync();

                fileStream.Close();

                OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(language);
                OcrResult result = await ocrEngine.RecognizeAsync(softwareBitmap);
                results.Add(result);
            }

            return results;
        }

        public static void ProcessResults(GameDecodedInfo gameDecodedInfo, List<OcrResult> results, uint observerId, ILogger<OcrService> logger)
        {
            foreach (OcrResult result in results) ProcessLines(gameDecodedInfo, result.Lines.ToList());

            if (FortniteOCR.debugMode) logger.LogDebug("\u001b[37mDEBUG -> " + JsonConvert.SerializeObject(gameDecodedInfo) + "\u001b[1m\u001b[37m");

            BackendClient.StoreData(observerId, gameDecodedInfo, logger);
        }

        private static void ProcessLines(GameDecodedInfo gameDecodedInfo, List<OcrLine> lines)
        {
            foreach (OcrLine line in lines)
            {
                string lineText = line.Text;
                gameDecodedInfo.SetPlayerName(lineText);
            }

            if (lines.Count() == 0)
            {
                gameDecodedInfo.SetPlayerName(null);
            }
        }
    }
}
