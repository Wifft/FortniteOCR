namespace FortniteOCR.Models
{
    internal sealed class ConsoleSpinner
    {
        static string[,]? sequence = null;

        public int Delay { get; set; } = 200;

        int totalSequences = 0;
        int counter;

        public ConsoleSpinner()
        {
            counter = 0;
            sequence = new string[,] {
                { "/", "-", "\\", "|" },
                { ".", "o", "0", "o" },
                { "+", "x","+","x" },
                { "V", "<", "^", ">" },
                { ".   ", "..  ", "... ", "...." },
                { "=>   ", "==>  ", "===> ", "====>" },
           };

            totalSequences = sequence.GetLength(0);
        }

        public void Turn(string displayMsg = "", int sequenceCode = 0)
        {
            counter++;

            Thread.Sleep(Delay);

            sequenceCode = sequenceCode > totalSequences - 1 ? 0 : sequenceCode;

            int counterValue = counter % 4;

            string fullMessage = displayMsg + sequence?[sequenceCode, counterValue];
            int msglength = fullMessage.Length;

            Console.Write(fullMessage);

            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
