using System.Drawing.Imaging;
using System.Drawing;

namespace FortniteOCR.Helpers
{
    internal static class ScreenReadingHelper
    {
        public static List<string> GetScreenshoots()
        {
            List<string> filenames = new();

            List<Rectangle> rectangles = new()
            {
                new(685, 840, 410, 55), //Player name.
            };

            foreach (Rectangle rectangle in rectangles)
            {
                int idx = rectangles.IndexOf(rectangle);
                string filename = Environment.GetEnvironmentVariable("tmp") + $"\\fortnite_ocr_game_capture_{idx}.jpg";

                Bitmap bitmap = new(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);

                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.CopyFromScreen(rectangle.Left, rectangle.Top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);

                bitmap.Save(filename);

                filenames.Add(filename);
            }

            return filenames;
        }
    }
}
