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

        /// <summary>
        /// 警告ヘッダーを目立つように表示します。呼び出されるたびに色が変わります。
        /// </summary>
        /// <param name="title">表示するタイトル</param>
        public static void PrintAttentionHeader(string title, (ConsoleColor Background, ConsoleColor Foreground) colors)
        {
            Console.WriteLine();

            Console.BackgroundColor = colors.Background;
            Console.ForegroundColor = colors.Foreground;

            string line = new('!', Console.WindowWidth > 1 ? Console.WindowWidth - 1 : 80);
            string emptyLineWithBg = new(' ', line.Length);

            // 中央揃えのタイトルを計算
            string centeredTitle = $"!!! {title.ToUpper()} !!!";
            int padding = (line.Length - centeredTitle.Length) / 2;
            string paddedTitle = centeredTitle.PadLeft(centeredTitle.Length + padding);

            // 高さを2倍にして出力
            Console.WriteLine(line);
            Console.WriteLine(line);
            Console.WriteLine(emptyLineWithBg);
            Console.WriteLine(paddedTitle);
            Console.WriteLine(emptyLineWithBg);
            Console.WriteLine(line);
            Console.WriteLine(line);

            Console.ResetColor(); // 色を元に戻す
        }
    }
}