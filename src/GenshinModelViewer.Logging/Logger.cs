using System.Diagnostics;
using System.Globalization;
using System.IO.Abstractions;
using System.Reflection;

namespace GenshinModelViewer.Logging
{
    public static class Logger
    {
        internal static readonly FileSystem FileSystem = new();
        internal static readonly IPath Path = FileSystem.Path;
        internal static readonly IDirectory Directory = FileSystem.Directory;

        public static readonly string ApplicationLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"GenshinModelViewer\logs");

        static Logger()
        {
            if (!Directory.Exists(ApplicationLogPath))
            {
                Directory.CreateDirectory(ApplicationLogPath);
            }

            string logFilePath = Path.Combine(ApplicationLogPath, DateTime.Now.ToString(@"yyyyMMdd", CultureInfo.InvariantCulture) + ".log");
            Trace.Listeners.Add(new TextWriterTraceListener(logFilePath));
            Trace.AutoFlush = true;
        }

        public static void Info(params object[] values)
        {
            Log("INFO", string.Join(" ", values));
        }

        public static void Warn(params object[] values)
        {
            Log("ERROR", string.Join(" ", values));
        }

        public static void Error(params object[] values)
        {
            Log("ERROR", string.Join(" ", values));
        }

        public static void Fatal(params object[] values)
        {
            Log("ERROR", string.Join(" ", values));
#if DEBUG
            Debugger.Break();
#endif
        }

        public static void Exception(Exception e, string message = null!)
        {
            Log(
                (message ?? string.Empty) + Environment.NewLine +
                e?.Message + Environment.NewLine +
                "Inner exception: " + Environment.NewLine +
                e?.InnerException?.Message + Environment.NewLine +
                "Stack trace: " + Environment.NewLine +
                e?.StackTrace,
                "ERROR");
#if DEBUG
            Debugger.Break();
#endif
        }

        private static void Log(string type, string message)
        {
            Trace.Write(type + "|" + DateTime.Now.ToString(@"yyyy-MM-dd|HH:mm:ss.fff", CultureInfo.InvariantCulture));
            Trace.Write("|" + GetCallerInfo());
            Trace.WriteLine("|" + message);
        }

        private static string GetCallerInfo()
        {
            StackTrace stackTrace = new();

            MethodBase methodName = stackTrace.GetFrame(3)?.GetMethod()!;
            string? className = methodName?.DeclaringType?.Name;
            return className + "|" + methodName?.Name;
        }
    }
}
