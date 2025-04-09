using AALUND13Card.Extensions;
using AALUND13Card.Utils;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.RandomStatGenerators {
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

        public static void CreateRandomStatsCard(string statGenName, int seed, string cardName, string cardDescription, int minRandomStat, int maxRandomStat, Player player = null, string twoLetterCode = null) {
            RandomSyncSeed.InvokeWithSeed(statGenName, seed, cardName, cardDescription, minRandomStat, maxRandomStat, player?.playerID ?? -1, twoLetterCode);
        }

        public static void CreateRandomStatsCard(string statGenName, string cardName, string cardDescription, int minRandomStat, int maxRandomStat, Player player = null, string twoLetterCode = null) {
            RandomSyncSeed.Invoke(statGenName, cardName, cardDescription, minRandomStat, maxRandomStat, player?.playerID ?? -1, twoLetterCode);
        }
    }

    public class RandomStatHandler {
        public List<RandomStatGenerator> StatGenerators { get; private set; }
        public readonly string StatGenName;

        public event Action<CardInfo, System.Random> OnCardGenerated;

        public RandomStatHandler(string statGenName, List<RandomStatGenerator> statGenerators) {
            StatGenName = statGenName;
            StatGenerators = statGenerators;
            RandomStatManager.RandomStatHandlers.Add(statGenName, this);

            RandomSyncSeed.RegisterSyncedRandom(statGenName, SyncRandomSyncStats);
        }

        public CardInfoStat[] GetCardStatsFromSeed(int seed, int minRandomStat, int maxRandomStat) {
            System.Random random = new System.Random(seed);

            int numberOfStats = random.Next(minRandomStat, maxRandomStat);
            Dictionary<RandomStatGenerator, float> selectedStats = new Dictionary<RandomStatGenerator, float>();

            while(selectedStats.Count < numberOfStats) {
                int index = random.Next(StatGenerators.Count);
                if(!selectedStats.ContainsKey(StatGenerators[index])) {
                    float value = random.NextFloat(StatGenerators[index].MinValue, StatGenerators[index].MaxValue);
                    if(!StatGenerators[index].ShouldApply(value)) continue;

                    selectedStats.Add(StatGenerators[index], value);
                }
            }

            List<CardInfoStat> cardStats = new List<CardInfoStat>();
            foreach(var item in selectedStats) {
                cardStats.Add(new CardInfoStat {
                    stat = item.Key.StatName,
                    amount = item.Key.GetStatString(item.Value),
                    positive = item.Key.IsPositive(item.Value)
                });
            }

            return cardStats.ToArray();
        }

        public void ApplyRandomStats(CardInfo cardInfo, System.Random random, string cardName, int minRandomStat, int maxRandomStat) {
            int adjustedMinRandomStat = Mathf.Max(0, minRandomStat);
            int adjustedMaxRandomStat = Mathf.Clamp(maxRandomStat, adjustedMinRandomStat, StatGenerators.Count);

            BuildRandomStatCard statCard = cardInfo.gameObject.AddComponent<BuildRandomStatCard>();
            statCard.CardName = cardName;

            Gun gun = cardInfo.GetComponent<Gun>();
            CharacterStatModifiers statModifiers = cardInfo.GetComponent<CharacterStatModifiers>();
            Block block = cardInfo.GetComponent<Block>();


            LoggerUtils.LogInfo($"Generating random stats for {cardInfo.cardName}...");
            int numberOfStats = random.Next(adjustedMinRandomStat, adjustedMaxRandomStat);
            Dictionary<RandomStatGenerator, float> selectedStats = new Dictionary<RandomStatGenerator, float>();

            while(selectedStats.Count < numberOfStats) {
                int index = random.Next(StatGenerators.Count);
                if(!selectedStats.ContainsKey(StatGenerators[index])) {
                    float value = random.NextFloat(StatGenerators[index].MinValue, StatGenerators[index].MaxValue);
                    if(!StatGenerators[index].ShouldApply(value)) continue;

                    selectedStats.Add(StatGenerators[index], value);
                }
            }


            List<CardInfoStat> cardStats = new List<CardInfoStat>();
            foreach(var item in selectedStats) {
                item.Key.Apply(item.Value, cardInfo.gameObject, gun, statModifiers, block);
                cardStats.Add(new CardInfoStat {
                    stat = item.Key.StatName,
                    amount = item.Key.GetStatString(item.Value),
                    positive = item.Key.IsPositive(item.Value)
                });
            }
            cardInfo.cardStats = cardStats.ToArray();
            LoggerUtils.LogInfo($"Generated {cardStats.Count} stats for {cardInfo.cardName}.");
        }

        public void GenerateRandomStats(System.Random random, string cardName, string cardDescription, int minRandomStat, int maxRandomStat, Player player = null, string twoLetterCode = null, Action<CardInfo, System.Random> onCardGenerated = null) {
            int adjustedMinRandomStat = Mathf.Max(0, minRandomStat);
            int adjustedMaxRandomStat = Mathf.Clamp(maxRandomStat, adjustedMinRandomStat, StatGenerators.Count);

            GameObject newCard = GameObject.Instantiate(AALUND13_Cards.BlankCardPrefab);
            GameObject.Destroy(newCard.transform.GetChild(0).gameObject);
            GameObject.DontDestroyOnLoad(newCard);

            CardInfo newCardInfo = newCard.GetComponent<CardInfo>();
            BuildRandomStatCard buildRandomStatCard = newCard.AddComponent<BuildRandomStatCard>();
            RandomStatManager.AddCardToGenerated(StatGenName, newCardInfo);

            newCardInfo.cardName = $"{StatGenName} Card ({RandomStatManager.GetGeneratedCards(StatGenName).Count})";
            newCardInfo.cardDestription = cardDescription;
            buildRandomStatCard.CardName = cardName;
            buildRandomStatCard.IsHidden = true;

            Gun gun = newCard.GetComponent<Gun>();
            CharacterStatModifiers statModifiers = newCard.GetComponent<CharacterStatModifiers>();
            Block block = newCard.GetComponent<Block>();

            LoggerUtils.LogInfo($"Generating random stats for {cardName}...");
            int numberOfStats = random.Next(adjustedMinRandomStat, adjustedMaxRandomStat);
            Dictionary<RandomStatGenerator, float> selectedStats = new Dictionary<RandomStatGenerator, float>();

            while(selectedStats.Count < numberOfStats) {
                int index = random.Next(StatGenerators.Count);
                if(!selectedStats.ContainsKey(StatGenerators[index])) {
                    float value = random.NextFloat(StatGenerators[index].MinValue, StatGenerators[index].MaxValue);
                    if(!StatGenerators[index].ShouldApply(value)) continue;

                    selectedStats.Add(StatGenerators[index], value);
                }
            }


            List<CardInfoStat> cardStats = new List<CardInfoStat>();
            foreach(var item in selectedStats) {
                item.Key.Apply(item.Value, newCard, gun, statModifiers, block);
                cardStats.Add(new CardInfoStat {
                    stat = item.Key.StatName,
                    amount = item.Key.GetStatString(item.Value),
                    positive = item.Key.IsPositive(item.Value)
                });
            }
            newCardInfo.cardStats = cardStats.ToArray();
            LoggerUtils.LogInfo($"Generated {cardStats.Count} stats for {cardName}.");

            LoggerUtils.LogInfo("Building card...");
            buildRandomStatCard.BuildUnityCard((cardInfo) => {
                buildRandomStatCard.Register(cardInfo);
                OnCardGenerated?.Invoke(cardInfo, random);
                onCardGenerated?.Invoke(cardInfo, random);

                if(player != null) {
                    AALUND13_Cards.Instance.ExecuteAfterSeconds(0.2f, () => {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, cardInfo, false, twoLetterCode, 2f, 2f, true);
                    });
                }

                LoggerUtils.LogInfo("Card built!");
            });
        }

        private void SyncRandomSyncStats(SyncedRandomContext context) {
            string cardName = (string)context.Parameters[0];
            string cardDescription = (string)context.Parameters[1];

            int minRandomStat = Mathf.Max(0, (int)context.Parameters[2]);
            int maxRandomStat = Mathf.Clamp((int)context.Parameters[3], minRandomStat, StatGenerators.Count);

            Player player = null;
            if(context.Parameters.Length > 4 && (int)context.Parameters[4] != -1) player = PlayerManager.instance.players.Find(p => p.playerID == (int)context.Parameters[4]);
            string twoLetterCode = context.Parameters.Length > 5 ? (string)context.Parameters[5] : null;

            GenerateRandomStats(context.Random, cardName, cardDescription, minRandomStat, maxRandomStat, player, twoLetterCode);
        }
    }
}
