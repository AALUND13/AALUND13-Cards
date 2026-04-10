using DrawNCards;
using HarmonyLib;
using PickPhaseImprovements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata;
using UnboundLib.GameModes;
using UnityEngine;
using static PickPhaseImprovements.PickManager;

namespace AALUND13Cards.Core.Handlers {
    public enum ExtraPickPhaseTrigger {
        TriggerInPlayerPickEnd,
        TriggerInPickEnd
    }


    public class ExtraPickHandler {
        public virtual bool PickConditions(Player player, CardInfo card) {
            return true;
        }

        public virtual void OnPickStart(Player player) { }
        public virtual void OnPickEnd(Player player, CardInfo card) { }

        public virtual int HandSize { get; set; } = 0;

        [Obsolete("Property Picks is deprecated. Use the number of times you call AddExtraPick instead to determine the number of picks.")]
        public int Picks { get; internal set; } = 0;
    }

    public static class ExtraCardPickHandler {
        internal static Dictionary<Player, Dictionary<Type, List<PickManager.ShuffleData>>> extraPicks =
            new Dictionary<Player, Dictionary<Type, List<PickManager.ShuffleData>>>();

        public static ExtraPickHandler activePickHandler;
        public static Player currentPlayer;

        [Obsolete("Method AddExtraPick with ExtraPickPhaseTrigger parameter is deprecated. Use AddExtraPick without the parameter instead.")]
        public static void AddExtraPick(ExtraPickHandler extraPickHandler, Player player, int picks, ExtraPickPhaseTrigger pickPhaseTrigger = ExtraPickPhaseTrigger.TriggerInPlayerPickEnd) {
            AddExtraPick(extraPickHandler, player, picks);
        }

        [Obsolete("Method AddExtraPick<T> with ExtraPickPhaseTrigger parameter is deprecated. Use AddExtraPick<T> without the parameter instead.")]
        public static void AddExtraPick<T>(Player player, int picks, ExtraPickPhaseTrigger pickPhaseTrigger = ExtraPickPhaseTrigger.TriggerInPlayerPickEnd) where T : ExtraPickHandler {
            AddExtraPick((ExtraPickHandler)Activator.CreateInstance(typeof(T)), player, picks);
        }


        public static void AddExtraPick<T>(Player player, int picks) where T : ExtraPickHandler {
            AddExtraPick((ExtraPickHandler)Activator.CreateInstance(typeof(T)), player, picks);
        }

        public static PickManager.ShuffleData RemoveExtraPick<T>(Player player) where T : ExtraPickHandler {
            if(!extraPicks.TryGetValue(player, out var typeDict)) throw new Exception($"Player '{player.playerID}' does not have any extra picks.");
            if(!typeDict.TryGetValue(typeof(T), out var shuffleList)) throw new Exception($"Player '{player.playerID}' does not have any extra picks of type '{typeof(T)}'.");
            if(shuffleList.Count <= 0) throw new Exception($"Player '{player.playerID}' does not have any extra picks of type '{typeof(T)}'.");

            PickManager.ShuffleData shuffleData = shuffleList[0];
            shuffleList.RemoveAt(0);
            return shuffleData;
        }

        public static PickManager.ShuffleData RemoveExtraPick(ExtraPickHandler handler, Player player) {
            return RemoveExtraPick(handler.GetType(), player);
        }

        public static PickManager.ShuffleData RemoveExtraPick(Type handlerType, Player player) {
            if(!extraPicks.TryGetValue(player, out var typeDict)) throw new Exception($"Player '{player.playerID}' does not have any extra picks.");
            if(!typeDict.TryGetValue(handlerType, out var shuffleList)) throw new Exception($"Player '{player.playerID}' does not have any extra picks of type '{handlerType}'.");
            if(shuffleList.Count <= 0) throw new Exception($"Player '{player.playerID}' does not have any extra picks of type '{handlerType}'.");

            PickManager.ShuffleData shuffleData = shuffleList[0];
            shuffleList.RemoveAt(0);
            return shuffleData;
        }

        public static bool HasExtraPick<T>(Player player) where T : ExtraPickHandler {
            return extraPicks.TryGetValue(player, out var typeDict) && typeDict.TryGetValue(typeof(T), out var shuffleList) && shuffleList.Count > 0;
        }

        public static bool HasExtraPick(ExtraPickHandler handler, Player player) {
            return extraPicks.TryGetValue(player, out var typeDict) && typeDict.TryGetValue(handler.GetType(), out var shuffleList) && shuffleList.Count > 0;
        }

        public static bool HasExtraPick(Player player, Type handlerType) {
            return extraPicks.TryGetValue(player, out var typeDict) && typeDict.TryGetValue(handlerType, out var shuffleList) && shuffleList.Count > 0;
        }

        public static void AddExtraPick(ExtraPickHandler handler, Player player, int picks) {
            for(int i = 0; i < picks; i++) {
                PickManager.ShuffleData shuffleData = new PickManager.ShuffleData() {
                    HandSize = handler.HandSize,
                    Relative = false,
                    Condition = (CardInfo info) => {
                        return handler.PickConditions(player, info);
                    },
                    pickStartCallback = () => {
                        handler.OnPickStart(player);

                        if(HasExtraPick(handler, player)) {
                            RemoveExtraPick(handler, player);
                        }
                    },
                    pickEndCallback = () => {
                        handler.OnPickEnd(player, PickManager.lastPickedCard);
                    }
                };
                PickManager.QueueShuffleForPicker(player, shuffleData);

                if(!extraPicks.TryGetValue(player, out var typeDict)) {
                    typeDict = new Dictionary<Type, List<PickManager.ShuffleData>>();
                    extraPicks[player] = typeDict;
                }

                if(!typeDict.TryGetValue(handler.GetType(), out var shuffleList)) {
                    shuffleList = new List<PickManager.ShuffleData>();
                    typeDict[handler.GetType()] = shuffleList;
                }

                shuffleList.Add(shuffleData);
            }
        }
    }
}
