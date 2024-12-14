using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Networking;
using UnityEngine;

namespace AALUND13Card.Cards {
    public class NegativeStatCard : CustomCardAACard {
        protected int? cardSeed;

        protected static readonly Dictionary<int, (Func<int, CardInfoStat>, Action<float, Gun, CharacterStatModifiers, Block>)> StatGenerators =
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

        protected List<(float, Action<float, Gun, CharacterStatModifiers, Block>)> StatAppliers = new List<(float, Action<float, Gun, CharacterStatModifiers, Block>)>();

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

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            StartCoroutine(Setup(cardInfo));
        }

        protected IEnumerator Setup(CardInfo card) {
            yield return GenerateNegativeStats(card);

            GetComponentInChildren<CardInfoDisplayer>().DrawCard(cardInfo.cardStats, GetTitle());
        }

        protected IEnumerator GenerateNegativeStats(CardInfo cardInfo, int? seed = null) {
            RandomStatTracker.GenerateRandomSeed();
            yield return new WaitUntil(() => seed != null || RandomStatTracker.Seed != null);
            cardSeed = RandomStatTracker.Seed.Value;

            System.Random random = new System.Random(seed ?? cardSeed.Value);
            int numberOfStats = random.Next(1, 4);

            var selectedStats = new List<(CardInfoStat, int)>();
            HashSet<int> selectedIndices = new HashSet<int>();

            while(selectedIndices.Count < numberOfStats) {
                int index = random.Next(StatGenerators.Count);
                if(selectedIndices.Add(index)) {
                    var generator = StatGenerators[index];
                    var stat = generator.Item1(random.Next());
                    StatAppliers.Add((float.Parse(stat.amount.Replace("%", "")) / 100, generator.Item2));
                    selectedStats.Add((stat, index));
                }
            }

            cardInfo.cardStats = selectedStats.Select(s => s.Item1).ToArray();
        }


        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            RandomStatTracker.Seed = cardSeed;
            CustomCard.BuildCard<BuildNegativeStatCard>(cardInfo => {
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(cardInfo);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, cardInfo, false, "", 2f, 2f, true);
            });

            CardRemover.RemoveCardFromPlayer(player, cardInfo.sourceCard, 10);
        }
    }

    public static class RandomStatTracker {
        public static int? Seed;
        public static readonly Dictionary<CardInfo, int> CreatedRandomStatsCards = new Dictionary<CardInfo, int>();

        public static void GenerateRandomSeed() {
            Seed = new System.Random().Next();
            NetworkingManager.RPC(typeof(RandomStatTracker), nameof(RPCA_SyncRandomSeed), Seed);
        }

        [UnboundRPC]
        private static void RPCA_SyncRandomSeed(int seed) {
            Seed = seed;
        }
    }

    public class BuildNegativeStatCard : NegativeStatCard {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            if(RandomStatTracker.CreatedRandomStatsCards.ContainsKey(cardInfo.sourceCard ?? cardInfo)) {
                cardSeed = RandomStatTracker.CreatedRandomStatsCards[cardInfo.sourceCard ?? cardInfo];
            } else {
                RandomStatTracker.CreatedRandomStatsCards.Add(cardInfo.sourceCard ?? cardInfo, RandomStatTracker.Seed.Value);
                cardSeed = RandomStatTracker.Seed;
            }

            StartCoroutine(ApplyNegativeStats(cardInfo));

            TextMeshProUGUI[] allChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            if(allChildren.Length > 0) {
                allChildren.Where(obj => obj.gameObject.name == "Text_Name").FirstOrDefault().GetComponent<TextMeshProUGUI>().text = "Defective Card";
            }
        }

        private IEnumerator ApplyNegativeStats(CardInfo cardInfo) {
            yield return GenerateNegativeStats(cardInfo, cardSeed.Value);
            foreach(var statApplier in StatAppliers) {
                UnityEngine.Debug.Log($"Applying stat: {statApplier.Item1}");
                statApplier.Item2(statApplier.Item1, gun, statModifiers, block);
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
        }

        protected override string GetTitle() {
            return $"Defective Card ({cardSeed})";
        }

        public override bool GetEnabled() {
            return false;
        }
    }
}
