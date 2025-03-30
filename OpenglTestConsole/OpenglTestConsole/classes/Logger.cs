using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Logger
    {

        public static void Log(string info, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Log(info, ConsoleColor.Blue);
                    break;
                case LogLevel.Warning:
                    Log(info, ConsoleColor.Yellow);
                    break;
                case LogLevel.Error:
                    Log(info, ConsoleColor.Red);
                    break;
                default:
                    Log(info, ConsoleColor.White);
                    break;
            }
            // since we change the color when we log, logging without us changing color means the app crashed, so we should set the color to red
            Console.ForegroundColor = ConsoleColor.Red;

        }

        public static void Log(string info, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(info);
        }

    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}
