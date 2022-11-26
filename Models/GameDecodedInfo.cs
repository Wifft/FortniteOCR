using System.Text.RegularExpressions;

namespace FortniteOCR.Models
{
    internal sealed class GameDecodedInfo
    {
        public string? playerName = null;

        public void SetPlayerName(string? playerName)
        {
            this.playerName = playerName;
        }
    }
}
