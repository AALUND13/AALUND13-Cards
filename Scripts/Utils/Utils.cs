namespace AALUND13Card {
    public static class Utils {
        public static bool logging = ConfigHandler.DebugMode.Value;

        public static void LogInfo(string message) { if(logging) UnityEngine.Debug.Log(message); }
        public static void LogWarn(string message) { if(logging) UnityEngine.Debug.LogWarning(message); }
        public static void LogError(string message) { if(logging) UnityEngine.Debug.LogError(message); }

        public static int CountOfAliveEnemyPlayers(Player player) {
            return ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(player).FindAll(__player => !__player.data.dead).Count;
        }
    }
}
