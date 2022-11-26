using System.Globalization;

namespace FortniteOCR.Helpers
{
    internal static class InstallationHelper
    {
        public static void CheckTimebomb()
        {
            DateTime expirationDate = DateTime.ParseExact("12/01/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if ((DateTime.Now > expirationDate))
            {
                Console.WriteLine("App expired!");
                Console.Write("Press ENTER to exit.");
                Console.Read();

                Environment.Exit(0);
            }
        }
    }
}
