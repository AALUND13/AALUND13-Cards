using AALUND13Card;
using AALUND13Card.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;

namespace AALUND13_Card.Utils {
    public static class NegativeStatCardGenerator {
        public static readonly Dictionary<int, (Func<int, CardInfoStat>, Action<float, Gun, CharacterStatModifiers, Block>)> StatGenerators =
            new Dictionary<int, (Func<int, CardInfoStat>, Action<float, Gun, CharacterStatModifiers, Block>)>
        {
            { 0, (
                random => GenerateCardStat("Damage", GetNegativeRandomValue(random, 50)),
                (value, gun, stats, block) => gun.damage *= 1 + value
            )},
            { 1, (
                random => GenerateCardStat("Reload Time", GetPositiveRandomValue(random, 50)),
                (value, gun, stats, block) => gun.reloadTime *= 1 + value
            )},
            { 2, (
                random => GenerateCardStat("Attack Speed", GetPositiveRandomValue(random, 50)),
                (value, gun, stats, block) => gun.attackSpeed *= 1 + value
            )},
            { 3, (
                random => GenerateCardStat("Movement Speed", GetNegativeRandomValue(random, 25)),
                (value, gun, stats, block) => stats.movementSpeed *= 1 + value
            )},
            { 4, (
                random => GenerateCardStat("Health", GetNegativeRandomValue(random, 50)),
                (value, gun, stats, block) => stats.health *= 1 + value
            )},
            { 5, (
                random => GenerateCardStat("Block Cooldown", GetPositiveRandomValue(random, 50)),
                (value, gun, stats, block) => block.cdMultiplier *= 1 + value
            )},
            { 6, (
                random => GenerateCardStat("Bullet Speed", GetNegativeRandomValue(random, 50)),
                (value, gun, stats, block) => gun.projectileSpeed *= 1 + value
            )}
        };

        private static CardInfoStat GenerateCardStat(string statName, float value) {
            return new CardInfoStat {
                stat = statName,
                amount = $"{(value > 0 ? "+" : "")}{value}%",
                positive = false
            };
        }

        private static float GetPositiveRandomValue(int random, int max) {
            return 1.0f + (random % max); // Random value between 1% and max%
        }

        private static float GetNegativeRandomValue(int random, int max) {
            return -1.0f * (1 + (random % max)); // Random value between -1% and -max%
        }

        internal static void Setup() {
            RandomSyncSeed.RegisterSyncedRandom("RegisterNegativeStatCard", (SyncedRandomContext) => {
                Player player = PlayerManager.instance.players.Find(p => p.playerID == (int)SyncedRandomContext.Parameters[0]);

                GameObject newCard = GameObject.Instantiate(AALUND13_Cards.BlankCardPrefab);
                GameObject.DestroyImmediate(newCard.transform.GetChild(0).gameObject);
                GameObject.DontDestroyOnLoad(newCard);

                CardInfo newCardInfo = newCard.GetComponent<CardInfo>();
                BuildNegativeStatCard customCard = newCard.AddComponent<BuildNegativeStatCard>();

                newCardInfo.cardName = $"Defective Card ({SyncedRandomContext.Random.Next(int.MaxValue)})";
                newCardInfo.cardDestription = "A defective card";

                Gun gun = newCard.GetComponent<Gun>();
                CharacterStatModifiers statModifiers = newCard.GetComponent<CharacterStatModifiers>();
                Block block = newCard.GetComponent<Block>();

                int numberOfStats = SyncedRandomContext.Random.Next(1, 4);

                var selectedStats = new List<(CardInfoStat, int)>();
                HashSet<int> selectedIndices = new HashSet<int>();

                LoggerUtils.LogInfo($"Generating {numberOfStats} stats...");
                while(selectedIndices.Count < numberOfStats) {
                    int index = SyncedRandomContext.Random.Next(StatGenerators.Count);
                    if(selectedIndices.Add(index)) {
                        var generator = StatGenerators[index];
                        var stat = generator.Item1(SyncedRandomContext.Random.Next());
                        var amount = float.Parse(stat.amount.Replace("%", "")) / 100;

                        generator.Item2(amount, gun, statModifiers, block);

                        selectedStats.Add((stat, index));
                    }
                }

                newCardInfo.cardStats = selectedStats.Select(stat => stat.Item1).ToArray();

                LoggerUtils.LogInfo("Building card...");
                customCard.BuildUnityCard((cardInfo) => {
                    ModdingUtils.Utils.Cards.instance.AddHiddenCard(cardInfo);

                    AALUND13_Cards.Instance.ExecuteAfterSeconds(0.2f, () => {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, cardInfo, false, "", 2f, 2f, true);
                    });

                    LoggerUtils.LogInfo("Card built!");
                });
            });
        }

        public static void AddDefectiveCard(Player player) {
            RandomSyncSeed.SyncSeed("RegisterNegativeStatCard", player.playerID);
        }
    }

    public static class RandomSyncSeed {
        private static readonly System.Random Random = new System.Random();

        public static void SyncSeed(string target, params object[] additionalParams) {
            NetworkingManager.RPC(typeof(RandomSyncSeed), nameof(RPCA_SyncSeed), Random.Next(), target, additionalParams);
        }

        [UnboundRPC]
        private static void RPCA_SyncSeed(int seed, string target, object[] additionalParams) {
            if(SyncedSeeds.ContainsKey(target)) {
                SyncedSeeds[target](new SyncedRandomContext(seed, additionalParams));
            }
        }

        public static Dictionary<string, Action<SyncedRandomContext>> SyncedSeeds = new Dictionary<string, Action<SyncedRandomContext>>();

        public static void RegisterSyncedRandom(string target, Action<SyncedRandomContext> action) {
            SyncedSeeds.Add(target, action);
        }
    }

    public class SyncedRandomContext {
        public System.Random Random { get; private set; }
        public object[] Parameters { get; private set; }

        public SyncedRandomContext(int seed, object[] parameters) {
            Random = new System.Random(seed);
            Parameters = parameters;
        }
    }

}
