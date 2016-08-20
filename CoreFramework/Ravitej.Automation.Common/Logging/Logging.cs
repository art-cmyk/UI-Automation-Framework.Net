using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Ravitej.Automation.Common.Logging
{
    public static class Logging
    {
        /// <summary>
        /// Write an Information type log entry
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        public static void WriteInfoLogEntry(string category, string message, params object[] args)
        {
            WriteLogEntry(LogX.Info, category, message, args);
        }

        /// <summary>
        /// Write a Verbose type log entry
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        public static void WriteVerboseLogEntry(string category, string message, params object[] args)
        {
            WriteLogEntry(LogX.Verbose, category, message, args);
        }

        /// <summary>
        /// Write a Warning type log entry.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        public static void WriteWarningLogEntry(string category, string message, params object[] args)
        {
            WriteLogEntry(LogX.Warning, category, message, args);
        }

        /// <summary>
        /// Write an Error type log entry.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        public static void WriteErrorLogEntry(string category, string message, params object[] args)
        {
            WriteLogEntry(LogX.Error, category, message, args);
        }

        /// <summary>
        /// Write a Critical type log entry.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        public static void WriteCriticalLogEntry(string category, string message, params object[] args)
        {
            WriteLogEntry(LogX.Critical, category, message, args);
        }

        private static void WriteLogEntry(LogMessageBuilder logObject, string category, string message, params object[] args)
        {
            logObject.Category(category).Write(message, args);
        }
    }
}
