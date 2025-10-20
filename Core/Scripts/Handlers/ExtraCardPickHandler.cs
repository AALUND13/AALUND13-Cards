using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Networking;
using UnityEngine;

namespace AALUND13Cards.Core.Handlers {
    public enum ExtraPickPhaseTrigger {
        TriggerInPlayerPickEnd,
        TriggerInPickEnd
    }

    public class ExtraPickHandler {
        public int Picks { get; internal set; } = 0;

        public virtual bool PickConditions(Player player, CardInfo card) {
            return true;
        }

        public virtual void OnPickStart(Player player) { }
        public virtual void OnPickEnd(Player player, CardInfo card) { } // This is a method that will be called after the player picks a card
    }

    public static class ExtraCardPickHandler {
        internal static Dictionary<Player, Dictionary<ExtraPickPhaseTrigger, ExtraPickHandler>> extraPicks = new Dictionary<Player, Dictionary<ExtraPickPhaseTrigger, ExtraPickHandler>>();

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
                extraPicks.Add(player, new Dictionary<ExtraPickPhaseTrigger, ExtraPickHandler>());
            }
            if(!extraPicks[player].ContainsKey(pickPhaseTrigger)) {
                extraPicks[player].Add(pickPhaseTrigger, new ExtraPickHandler());
            }
            extraPicks[player][pickPhaseTrigger].Picks += picks;
        }

        [UnboundRPC]
        public static void RPCA_RemoveExtraPick(int playerId, string handlerType, int picks) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            if(player == null) return;

            Type type = Type.GetType(handlerType);
            if(type == null) return;

            int pickToRemove = picks;
            if(extraPicks.TryGetValue(player, out var triggerDict)) {
                foreach(var handler in triggerDict.ToList()) {
                    if(pickToRemove <= 0) break;

                    int removeCount = Math.Min(handler.Value.Picks, pickToRemove);
                    handler.Value.Picks -= removeCount;
                    pickToRemove -= removeCount;

                    if(handler.Value.Picks <= 0) triggerDict.Remove(handler.Key);
                }
            }
        }


        internal static IEnumerator HandleExtraPicks(ExtraPickPhaseTrigger pickPhaseTrigger) {
            foreach(Player player in PlayerManager.instance.players.ToArray()) {
                if(extraPicks.ContainsKey(player) && extraPicks[player].ContainsKey(pickPhaseTrigger) && extraPicks[player][pickPhaseTrigger].Picks > 0) {
                    var pickHandlers = extraPicks[player][pickPhaseTrigger];
                    
                    if(pickPhaseTrigger == ExtraPickPhaseTrigger.TriggerInPickEnd) {
                        while(extraPicks.ContainsKey(player) && extraPicks[player].ContainsKey(pickPhaseTrigger) && extraPicks[player][pickPhaseTrigger].Picks > 0) {
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
            activePickHandler = extraPicks[player][pickPhaseTrigger];

            activePickHandler.OnPickStart(currentPlayer);
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
            CardChoiceVisuals.instance.Show(player.playerID, true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);

            // check if the player sill has extra picks to prevent 'ArgumentOutOfRangeException: Index was out of range.
            // Must be non-negative and less than the size of the collection.' error
            if(extraPicks.ContainsKey(player) && extraPicks[player].ContainsKey(pickPhaseTrigger) && extraPicks[player][pickPhaseTrigger].Picks > 0) {
                extraPicks[player][pickPhaseTrigger].Picks -= 1;
            }

            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
        }
    }
}
