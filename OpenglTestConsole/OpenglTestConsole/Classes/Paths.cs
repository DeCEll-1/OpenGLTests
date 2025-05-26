namespace OpenglTestConsole.Classes
{
    internal class Paths
    {
        public static DirectoryInfo ExeLocation { get => new(AppDomain.CurrentDomain.BaseDirectory); }
        public static DirectoryInfo ScreenshotLocation { get => new(ExeLocation + "Screenshots\\"); }

    }
}
