using ModdingUtils.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Networking;
using UnityEngine;

namespace AALUND13Card.Handlers {
    public class ExtraPickHandler {
        public virtual bool OnExtraPickStart(Player player, CardInfo card) { 
            return true; 
        }
        public virtual void OnExtraPick(Player player, CardInfo card) { } // This is a method that will be called after the player picks a card
    }

    public static class ExtraCardPickHandler {
        internal static Dictionary<Player, List<ExtraPickHandler>> extraPicks = new Dictionary<Player, List<ExtraPickHandler>>();
        public static Player currentPlayer;

        public static void AddExtraPick(ExtraPickHandler extraPickHandler, Player player, int picks) {
            NetworkingManager.RPC(typeof(ExtraCardPickHandler), nameof(RPCA_AddExtraPick), player.playerID, extraPickHandler.GetType().AssemblyQualifiedName, picks);
        }

        public static void AddExtraPick<T>(Player player, int picks) where T : ExtraPickHandler {
            NetworkingManager.RPC(typeof(ExtraCardPickHandler), nameof(RPCA_AddExtraPick), player.playerID, typeof(T).AssemblyQualifiedName, picks);
        }

        [UnboundRPC]
        private static void RPCA_AddExtraPick(int playerId, string handlerType, int picks) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            if(player == null) return;

            Type type = Type.GetType(handlerType);
            if(type == null) return;

            ExtraPickHandler handler = (ExtraPickHandler)Activator.CreateInstance(type);

            if(!extraPicks.ContainsKey(player)) {
                extraPicks.Add(player, new List<ExtraPickHandler>());
            }
            for(int i = 0; i < picks; i++) {
                extraPicks[player].Add(handler);
            }
        }

        internal static IEnumerator HandleExtraPicks() {
            foreach(Player player in PlayerManager.instance.players.ToArray()) {
                if(extraPicks.ContainsKey(player) && extraPicks[player].Count > 0) {
                    yield return HandleExtraPickForPlayer(player);;
                }
            }
            currentPlayer = null;
            yield break;
        }

        private static IEnumerator HandleExtraPickForPlayer(Player player) {
            currentPlayer = player;
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
            CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);
            extraPicks[player].RemoveAt(0);
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
        }
    }
}
