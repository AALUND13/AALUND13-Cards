namespace AALUND13Card {
    public static class LoggerUtils {
        public static bool logging = ConfigHandler.DebugMode.Value;

        public static void LogInfo(string message) { if(logging) AALUND13_Cards.logger.LogInfo(message); }
        public static void LogWarn(string message) { if(logging) AALUND13_Cards.logger.LogWarning(message); }
        public static void LogError(string message) { if(logging) AALUND13_Cards.logger.LogError(message); }
    }
}
