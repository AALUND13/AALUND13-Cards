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
        internal static Dictionary<Player, Dictionary<ExtraPickPhaseTrigger, Dictionary<Type, ExtraPickHandler>>> extraPicks =
            new Dictionary<Player, Dictionary<ExtraPickPhaseTrigger, Dictionary<Type, ExtraPickHandler>>>();

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

            if(!extraPicks.ContainsKey(player)) {
                extraPicks[player] = new Dictionary<ExtraPickPhaseTrigger, Dictionary<Type, ExtraPickHandler>>();
            }
            if(!extraPicks[player].ContainsKey(pickPhaseTrigger)) {
                extraPicks[player][pickPhaseTrigger] = new Dictionary<Type, ExtraPickHandler>();
            }

            if(!extraPicks[player][pickPhaseTrigger].TryGetValue(type, out var handler)) {
                handler = (ExtraPickHandler)Activator.CreateInstance(type);
                extraPicks[player][pickPhaseTrigger][type] = handler;
            }

            handler.Picks += picks;
        }

        [UnboundRPC]
        public static void RPCA_RemoveExtraPick(int playerId, string handlerType, int picks) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            if(player == null) return;

            Type type = Type.GetType(handlerType);
            if(type == null) return;

            if(extraPicks.TryGetValue(player, out var triggerDict)) {
                foreach(var phase in triggerDict.ToList()) {
                    if(phase.Value.TryGetValue(type, out var handler)) {
                        int removeCount = Math.Min(handler.Picks, picks);
                        handler.Picks -= removeCount;
                        picks -= removeCount;

                        if(handler.Picks <= 0)
                            phase.Value.Remove(type);
                    }

                    if(phase.Value.Count == 0)
                        triggerDict.Remove(phase.Key);

                    if(picks <= 0) break;
                }
            }
        }


        internal static IEnumerator HandleExtraPicks(ExtraPickPhaseTrigger pickPhaseTrigger) {
            foreach(var player in PlayerManager.instance.players.ToArray()) {
                if(!extraPicks.TryGetValue(player, out var triggerDict)) continue;
                if(!triggerDict.TryGetValue(pickPhaseTrigger, out var handlers)) continue;

                foreach(var handlerPair in handlers.ToList()) {
                    var handler = handlerPair.Value;
                    if(pickPhaseTrigger == ExtraPickPhaseTrigger.TriggerInPickEnd) {
                        while(handler.Picks > 0) {
                            yield return HandleExtraPickForPlayer(player, pickPhaseTrigger, handler);
                        }
                    } else if (handler.Picks > 0) {
                        yield return HandleExtraPickForPlayer(player, pickPhaseTrigger, handler);
                    }
                }
            }

            currentPlayer = null;
            activePickHandler = null;
        }

        private static IEnumerator HandleExtraPickForPlayer(Player player, ExtraPickPhaseTrigger pickPhaseTrigger, ExtraPickHandler handler) {
            currentPlayer = player;
            activePickHandler = handler;

            handler.OnPickStart(player);
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);

            CardChoiceVisuals.instance.Show(player.playerID, true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);

            handler.Picks = Mathf.Max(0, handler.Picks - 1);

            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
        }
    }
}
