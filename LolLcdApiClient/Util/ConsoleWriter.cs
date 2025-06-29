namespace LolLcdApiClient.Util
{
    /// <summary>
    /// コンソールへの出力機能
    /// </summary>
    public static class ConsoleWriter
    {
        /// <summary>
        /// 改行なしでメッセージを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Print(string message)
        {
            Console.Write(message);
        }

        /// <summary>
        /// 改行ありでメッセージを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// 改行ありで警告メッセージを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void PrintWarningLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// 改行ありでエラーメッセージを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void PrintErrorLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// 改行ありで文字色を指定してメッセージを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="color">文字色</param>
        public static void PrintLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            Console.ResetColor();
        }
    }
}