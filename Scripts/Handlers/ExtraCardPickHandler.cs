using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Networking;
using UnityEngine;

namespace AALUND13Cards.Handlers {
    public enum ExtraPickPhaseTrigger {
        TriggerInPlayerPickEnd,
        TriggerInPickEnd
    }

    public class ExtraPickHandler {
        public virtual bool OnExtraPickStart(Player player, CardInfo card) {
            return true;
        }
        public virtual void OnExtraPick(Player player, CardInfo card) { } // This is a method that will be called after the player picks a card
    }

    public static class ExtraCardPickHandler {
        internal static Dictionary<Player, Dictionary<ExtraPickPhaseTrigger, List<ExtraPickHandler>>> extraPicks = new Dictionary<Player, Dictionary<ExtraPickPhaseTrigger, List<ExtraPickHandler>>>();

        public static ExtraPickHandler activePickHandler;
        public static Player currentPlayer;

        public static void AddExtraPick(ExtraPickHandler extraPickHandler, Player player, int picks, ExtraPickPhaseTrigger pickPhaseTrigger = ExtraPickPhaseTrigger.TriggerInPlayerPickEnd) {
            NetworkingManager.RPC(typeof(ExtraCardPickHandler), nameof(RPCA_AddExtraPick), player.playerID, extraPickHandler.GetType().AssemblyQualifiedName, picks, pickPhaseTrigger);
        }

        public static void AddExtraPick<T>(Player player, int picks, ExtraPickPhaseTrigger pickPhaseTrigger = ExtraPickPhaseTrigger.TriggerInPlayerPickEnd) where T : ExtraPickHandler {
            NetworkingManager.RPC(typeof(ExtraCardPickHandler), nameof(RPCA_AddExtraPick), player.playerID, typeof(T).AssemblyQualifiedName, picks, pickPhaseTrigger);
        }

        public static void RemoveExtraPick(ExtraPickHandler extraPickHandler, Player player, int picks) {
            NetworkingManager.RPC(typeof(ExtraCardPickHandler), nameof(RPCA_RemoveExtraPick), player.playerID, extraPickHandler.GetType().AssemblyQualifiedName, picks);
        }

        public static void RemoveExtraPick<T>(Player player, int picks) where T : ExtraPickHandler {
            NetworkingManager.RPC(typeof(ExtraCardPickHandler), nameof(RPCA_RemoveExtraPick), player.playerID, typeof(T).AssemblyQualifiedName, picks);
        }

        [UnboundRPC]
        private static void RPCA_AddExtraPick(int playerId, string handlerType, int picks, ExtraPickPhaseTrigger pickPhaseTrigger) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            if(player == null) return;

            Type type = Type.GetType(handlerType);
            if(type == null) return;

            ExtraPickHandler handler = (ExtraPickHandler)Activator.CreateInstance(type);

            if(!extraPicks.ContainsKey(player)) {
                extraPicks.Add(player, new Dictionary<ExtraPickPhaseTrigger, List<ExtraPickHandler>>());
            }
            if(!extraPicks[player].ContainsKey(pickPhaseTrigger)) {
                extraPicks[player].Add(pickPhaseTrigger, new List<ExtraPickHandler>());
            }

            for(int i = 0; i < picks; i++) {
                extraPicks[player][pickPhaseTrigger].Add(handler);
            }
        }

        [UnboundRPC]
        public static void RPCA_RemoveExtraPick(int playerId, string handlerType, int picks) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            if(player == null) return;

            Type type = Type.GetType(handlerType);
            if(type == null) return;

            if(extraPicks.TryGetValue(player, out var triggerDict)) {
                int picksToRemove = picks;
                var emptyTriggers = new List<ExtraPickPhaseTrigger>();

                foreach(var trigger in triggerDict.Keys.ToList()) {
                    var handlers = triggerDict[trigger];

                    var toRemove = handlers.Where(h => h.GetType() == type)
                                           .Take(picksToRemove)
                                           .ToList();

                    foreach(var handler in toRemove) {
                        handlers.Remove(handler);
                        picksToRemove--;
                    }

                    if(handlers.Count == 0) {
                        emptyTriggers.Add(trigger);
                    }

                    if(picksToRemove <= 0) break;
                }

                foreach(var trigger in emptyTriggers) {
                    triggerDict.Remove(trigger);
                }

                if(triggerDict.Count == 0) {
                    extraPicks.Remove(player);
                }
            }
        }


        internal static IEnumerator HandleExtraPicks(ExtraPickPhaseTrigger pickPhaseTrigger) {
            foreach(Player player in PlayerManager.instance.players.ToArray()) {
                if(extraPicks.ContainsKey(player) && extraPicks[player].ContainsKey(pickPhaseTrigger) && extraPicks[player][pickPhaseTrigger].Count > 0) {
                    if(pickPhaseTrigger == ExtraPickPhaseTrigger.TriggerInPickEnd) {
                        while(extraPicks.ContainsKey(player) && extraPicks[player].ContainsKey(pickPhaseTrigger) && extraPicks[player][pickPhaseTrigger].Count > 0) {
                            yield return HandleExtraPickForPlayer(player, pickPhaseTrigger);
                        }
                    } else {
                        yield return HandleExtraPickForPlayer(player, pickPhaseTrigger);
                    }
                }
            }

            currentPlayer = null;
            activePickHandler = null;

            yield break;
        }

        private static IEnumerator HandleExtraPickForPlayer(Player player, ExtraPickPhaseTrigger pickPhaseTrigger) {
            currentPlayer = player;
            activePickHandler = extraPicks[player][pickPhaseTrigger][0];

            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
            CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);

            // check if the player sill has extra picks to prevent 'ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.' error
            if(extraPicks.ContainsKey(player) && extraPicks[player].ContainsKey(pickPhaseTrigger) && extraPicks[player][pickPhaseTrigger].Count > 0) {
                extraPicks[player][pickPhaseTrigger].RemoveAt(0);
            }

            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
        }
    }
}
