using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TifLib
{

    public class LogLevel
    {
        public const int DEBUG = 0;
        public const int INFO = 1;
        public const int WARN = 2;
        public const int CRITICAL = 3;
    }

    public class Logger
    {
        public int logLevel;
        public bool silent = false;
        public string logText;
        public string loggerName;

        public Logger(string loggerName = "noName", int logLevel = LogLevel.DEBUG)
        {
            this.logLevel = logLevel;
            this.loggerName = loggerName;
            Log($"starting logger: {loggerName}", LogLevel.DEBUG);
        }

        public void Debug(string message) { Log(message, LogLevel.DEBUG); }
        public void Info(string message) { Log(message, LogLevel.INFO); }
        public void Warn(string message) { Log(message, LogLevel.WARN); }
        public void Critical(string message) { Log(message, LogLevel.CRITICAL); }

        private void Log(string message, int logLevel)
        {
            if (logLevel > LogLevel.CRITICAL) logLevel = LogLevel.CRITICAL;
            if (logLevel < LogLevel.DEBUG) logLevel = LogLevel.DEBUG;
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string[] logLevelNames = { "DEBUG", "INFO", "WARN", "CRITICAL" };
            string logLevelName = logLevelNames[logLevel];
            string logLine = $"{timestamp} [{loggerName}] ({logLevelName}): {message}";
            logText = logText + logLine + "\n";
            if (!this.silent)
                System.Console.WriteLine(logLine);
        }
    }
}
