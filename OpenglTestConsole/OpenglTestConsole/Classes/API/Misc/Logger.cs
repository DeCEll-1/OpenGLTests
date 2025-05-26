namespace OpenglTestConsole.Classes.API.Misc
{
    public class Logger
    {
        public static void Log(string info, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Log(info, LogColors.BRIGHT_BLUE);
                    break;
                case LogLevel.Detail:
                    Log(info, LogColors.BRIGHT_BLACK);
                    break;
                case LogLevel.Warning:
                    Log(info, LogColors.BRIGHT_YELLOW);
                    break;
                case LogLevel.Error:
                    Log(info, LogColors.BRIGHT_RED);
                    break;
                default:
                    Log(info, LogColors.BRIGHT_WHITE);
                    break;
            }
            // since we change the color when we log, logging without us changing color means the app crashed, so we should set the color to red
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void Log(string info, string color)
        {
            // replace return to normals with the current color so we can change the color of texts
            Console.WriteLine(color + info.Replace(LogColors.NORMAL, color));
        }
    }

    public enum LogLevel
    {
        Info,
        Detail,
        Warning,
        Error,
    }

    public class LogColors
    {
        // https://en.wikipedia.org/wiki/ANSI_escape_code#Colors
        #region Colors
        public static string NORMAL = Console.IsOutputRedirected ? "" : "RETURN_TO_NORMAL";
        public static string BLACK = Console.IsOutputRedirected ? "" : "\x1b[30m";
        public static string RED = Console.IsOutputRedirected ? "" : "\x1b[31m";
        public static string GREEN = Console.IsOutputRedirected ? "" : "\x1b[32m";
        public static string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[33m";
        public static string BLUE = Console.IsOutputRedirected ? "" : "\x1b[34m";
        public static string MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[35m";
        public static string CYAN = Console.IsOutputRedirected ? "" : "\x1b[36m";
        public static string WHITE = Console.IsOutputRedirected ? "" : "\x1b[37m";
        public static string BRIGHT_BLACK = Console.IsOutputRedirected ? "" : "\x1b[90m";
        public static string BRIGHT_RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
        public static string BRIGHT_GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
        public static string BRIGHT_YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";
        public static string BRIGHT_BLUE = Console.IsOutputRedirected ? "" : "\x1b[94m";
        public static string BRIGHT_MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[95m";
        public static string BRIGHT_CYAN = Console.IsOutputRedirected ? "" : "\x1b[96m";
        public static string BRIGHT_WHITE = Console.IsOutputRedirected ? "" : "\x1b[97m";
        public static string BLACK_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[40m";
        public static string RED_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[41m";
        public static string GREEN_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[42m";
        public static string YELLOW_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[43m";
        public static string BLUE_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[44m";
        public static string MAGENTA_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[45m";
        public static string CYAN_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[46m";
        public static string WHITE_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[47m";
        public static string BRIGHT_BLACK_BACKGROUND = Console.IsOutputRedirected
            ? ""
            : "\x1b[100m";
        public static string BRIGHT_RED_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[101m";
        public static string BRIGHT_GREEN_BACKGROUND = Console.IsOutputRedirected
            ? ""
            : "\x1b[102m";
        public static string BRIGHT_YELLOW_BACKGROUND = Console.IsOutputRedirected
            ? ""
            : "\x1b[103m";
        public static string BRIGHT_BLUE_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[104m";
        public static string BRIGHT_MAGENTA_BACKGROUND = Console.IsOutputRedirected
            ? ""
            : "\x1b[105m";
        public static string BRIGHT_CYAN_BACKGROUND = Console.IsOutputRedirected ? "" : "\x1b[106m";
        public static string BRIGHT_WHITE_BACKGROUND = Console.IsOutputRedirected
            ? ""
            : "\x1b[107m";
        #endregion

        #region AutoGenFunctions
        public static string Black(object source) => BLACK + source.ToString() + NORMAL;

        public static string Red(object source) => RED + source.ToString() + NORMAL;

        public static string Green(object source) => GREEN + source.ToString() + NORMAL;

        public static string Yellow(object source) => YELLOW + source.ToString() + NORMAL;

        public static string Blue(object source) => BLUE + source.ToString() + NORMAL;

        public static string Magenta(object source) => MAGENTA + source.ToString() + NORMAL;

        public static string Cyan(object source) => CYAN + source.ToString() + NORMAL;

        public static string White(object source) => WHITE + source.ToString() + NORMAL;

        public static string BrightBlack(object source) =>
            BRIGHT_BLACK + source.ToString() + NORMAL;

        public static string BrightRed(object source) => BRIGHT_RED + source.ToString() + NORMAL;

        public static string BrightGreen(object source) =>
            BRIGHT_GREEN + source.ToString() + NORMAL;

        public static string BrightYellow(object source) =>
            BRIGHT_YELLOW + source.ToString() + NORMAL;

        public static string BrightBlue(object source) => BRIGHT_BLUE + source.ToString() + NORMAL;

        public static string BrightMagenta(object source) =>
            BRIGHT_MAGENTA + source.ToString() + NORMAL;

        public static string BrightCyan(object source) => BRIGHT_CYAN + source.ToString() + NORMAL;

        public static string BrightWhite(object source) =>
            BRIGHT_WHITE + source.ToString() + NORMAL;

        public static string BlackBackground(object source) =>
            BLACK_BACKGROUND + source.ToString() + NORMAL;

        public static string RedBackground(object source) =>
            RED_BACKGROUND + source.ToString() + NORMAL;

        public static string GreenBackground(object source) =>
            GREEN_BACKGROUND + source.ToString() + NORMAL;

        public static string YellowBackground(object source) =>
            YELLOW_BACKGROUND + source.ToString() + NORMAL;

        public static string BlueBackground(object source) =>
            BLUE_BACKGROUND + source.ToString() + NORMAL;

        public static string MagentaBackground(object source) =>
            MAGENTA_BACKGROUND + source.ToString() + NORMAL;

        public static string CyanBackground(object source) =>
            CYAN_BACKGROUND + source.ToString() + NORMAL;

        public static string WhiteBackground(object source) =>
            WHITE_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightBlackBackground(object source) =>
            BRIGHT_BLACK_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightRedBackground(object source) =>
            BRIGHT_RED_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightGreenBackground(object source) =>
            BRIGHT_GREEN_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightYellowBackground(object source) =>
            BRIGHT_YELLOW_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightBlueBackground(object source) =>
            BRIGHT_BLUE_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightMagentaBackground(object source) =>
            BRIGHT_MAGENTA_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightCyanBackground(object source) =>
            BRIGHT_CYAN_BACKGROUND + source.ToString() + NORMAL;

        public static string BrightWhiteBackground(object source) =>
            BRIGHT_WHITE_BACKGROUND + source.ToString() + NORMAL;
        #endregion

        #region Functions
        public static string Surround(string source, string surroundValue)
        {
            return surroundValue + source + NORMAL;
        }
        #endregion
    }
}
