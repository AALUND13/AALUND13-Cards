﻿using AALUND13Card.Extensions;
using System;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Utils.RandomStatsGenerator {
    public static class RandomStatManager {
        public readonly static Dictionary<string, RandomStatHandler> RandomStatHandlers = new Dictionary<string, RandomStatHandler>();
        public readonly static Dictionary<string, List<CardInfo>> GeneratedCards = new Dictionary<string, List<CardInfo>>();

        public static void AddRandomStatHandler(string statGenName, List<RandomStatGenerator> statGenerators) {
            RandomStatHandlers.Add(statGenName, new RandomStatHandler(statGenName, statGenerators));
        }

        internal static void AddCardToGenerated(string statGenName, CardInfo cardInfo) {
            if(GeneratedCards.ContainsKey(statGenName)) {
                GeneratedCards[statGenName].Add(cardInfo);
            } else {
                GeneratedCards.Add(statGenName, new List<CardInfo> { cardInfo });
            }
        }

        internal static List<CardInfo> GetGeneratedCards(string statGenName) {
            if(GeneratedCards.ContainsKey(statGenName)) {
                return GeneratedCards[statGenName];
            }
            return new List<CardInfo>();
        }

        public static void CreateRandomStatsCard(string statGenName, string cardName, string cardDescription, int minRandomStat, int maxRandomStat, Player player = null) {
            RandomSyncSeed.SyncSeed(statGenName, cardName, cardDescription, minRandomStat, maxRandomStat, player?.playerID ?? -1);
        }
    }

    public class RandomStatHandler {
        public List<RandomStatGenerator> StatGenerators { get; private set; }
        public readonly string StatGenName;

        public event Action<CardInfo, SyncedRandomContext> OnCardGenerated;

        public RandomStatHandler(string statGenName, List<RandomStatGenerator> statGenerators) {
            StatGenName = statGenName;
            StatGenerators = statGenerators;
            RandomStatManager.RandomStatHandlers.Add(statGenName, this);

            RandomSyncSeed.RegisterSyncedRandom(statGenName, SyncRandomSyncStats);
        }

        private void SyncRandomSyncStats(SyncedRandomContext context) {
            string cardName = (string)context.Parameters[0];
            string cardDescription = (string)context.Parameters[1];

            int minRandomStat = Mathf.Max(0, (int)context.Parameters[2]);
            int maxRandomStat = Mathf.Clamp((int)context.Parameters[3], minRandomStat, StatGenerators.Count);

            Player player = null;
            if(context.Parameters.Length > 4 && (int)context.Parameters[4] != -1) player = PlayerManager.instance.players.Find(p => p.playerID == (int)context.Parameters[4]);

            GameObject newCard = GameObject.Instantiate(AALUND13_Cards.BlankCardPrefab);
            GameObject.DestroyImmediate(newCard.transform.GetChild(0).gameObject);
            GameObject.DontDestroyOnLoad(newCard);

            CardInfo newCardInfo = newCard.GetComponent<CardInfo>();
            BuildRandomStatCard buildRandomStatCard = newCard.AddComponent<BuildRandomStatCard>();
            RandomStatManager.AddCardToGenerated(StatGenName, newCardInfo);

            newCardInfo.cardName = $"{cardName} ({RandomStatManager.GetGeneratedCards(StatGenName).Count})";
            newCardInfo.cardDestription = cardDescription;
            buildRandomStatCard.CardName = cardName;

            Gun gun = newCard.GetComponent<Gun>();
            CharacterStatModifiers statModifiers = newCard.GetComponent<CharacterStatModifiers>();
            Block block = newCard.GetComponent<Block>();

            LoggerUtils.LogInfo($"Generating random stats for {cardName}...");
            int numberOfStats = context.Random.Next(minRandomStat, maxRandomStat);
            List<RandomStatGenerator> selectedStats = new List<RandomStatGenerator>();

            while(selectedStats.Count < numberOfStats) {
                int index = context.Random.Next(StatGenerators.Count);
                if(!selectedStats.Contains(StatGenerators[index])) {
                    selectedStats.Add(StatGenerators[index]);
                }
            }

            List<CardInfoStat> cardStats = new List<CardInfoStat>();
            foreach(var item in selectedStats) {
                float value = context.Random.NextFloat(item.MinValue, item.MaxValue);
                cardStats.Add(new CardInfoStat {
                    stat = item.StatName,
                    amount = item.Apply(value, gun, statModifiers, block),
                    positive = item.IsPositive(value)
                });
            }
            newCardInfo.cardStats = cardStats.ToArray();
            LoggerUtils.LogInfo($"Generated {cardStats.Count} stats for {cardName}.");

            LoggerUtils.LogInfo("Building card...");
            buildRandomStatCard.BuildUnityCard((cardInfo) => {
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(cardInfo);
                OnCardGenerated?.Invoke(cardInfo, context);

                if(player != null) {
                    AALUND13_Cards.Instance.ExecuteAfterSeconds(0.2f, () => {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, cardInfo, false, "", 2f, 2f, true);
                    });
                }

                LoggerUtils.LogInfo("Card built!");
            });
        }
    }
}