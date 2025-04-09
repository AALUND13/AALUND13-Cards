using AALUND13Card.Handlers;

namespace AALUND13Card {
    public static class LoggerUtils {
        public static bool logging = ConfigHandler.DebugMode.Value;

        public static void LogInfo(string message) { if(logging) AALUND13_Cards.ModLogger.LogInfo(message); }
        public static void LogWarn(string message) { AALUND13_Cards.ModLogger.LogWarning(message); }
        public static void LogError(string message) { AALUND13_Cards.ModLogger.LogError(message); }
    }
}
