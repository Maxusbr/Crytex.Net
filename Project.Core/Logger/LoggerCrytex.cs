using NLog;

namespace Project.Core
{
    public static class LoggerCrytex
    {
        static LoggerCrytex()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        private static Logger _logger { get; }

        public static Logger Logger => _logger;

        public static void SetUserId(string userId)
        {
            MappedDiagnosticsContext.Set("user_id", userId);
        }

        public static void SetSource(SourceLog sourceLog)
        {
            MappedDiagnosticsContext.Set("source", sourceLog.ToString());
        }
    }
}