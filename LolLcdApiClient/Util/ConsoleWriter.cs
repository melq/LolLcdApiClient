namespace LolLcdApiClient.Util
{
    public static class ConsoleWriter
    {
        public static void Print(string message)
        {
            Console.Write(message);
        }

        public static void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void PrintWarningLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintErrorLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            Console.ResetColor();
        }
    }
}