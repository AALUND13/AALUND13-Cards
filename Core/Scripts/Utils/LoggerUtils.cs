using AALUND13Cards.Core.Handlers;

namespace AALUND13Cards.Core {
    public static class LoggerUtils {
        public static bool logging = ConfigHandler.DebugMode.Value;

        public static void LogInfo(string message) { if(logging) AAC_Core.ModLogger.LogInfo(message); }
        public static void LogWarn(string message) { AAC_Core.ModLogger.LogWarning(message); }
        public static void LogError(string message) { AAC_Core.ModLogger.LogError(message); }
    }
}
