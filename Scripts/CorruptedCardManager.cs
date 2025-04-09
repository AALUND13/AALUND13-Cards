using AALUND13Card.Cards;
using AALUND13Card.RandomStatGenerators;
using Photon.Pun;
using RarityLib.Utils;
using System;
using UnityEngine;

namespace AALUND13Card.Scripts {
    public class CorruptedCardManager : MonoBehaviour {
        public CardInfo[] CorruptedCardPrefabs;
        public static CorruptedCardManager Instance;

        internal void Init() {
            Instance = this;

            foreach(CardInfo card in CorruptedCardPrefabs) {
                card.rarity = RarityUtils.GetRarity(card.GetComponent<CorruptedCard>().Rarity.ToString());
                PhotonNetwork.PrefabPool.RegisterPrefab(card.name, card.gameObject);
            }
        }

        public void GiveCorruptedCard(Player player, string rarity) {
            string statGenName = $"CorruptedStatGenerator_{rarity}";
            if(!RandomStatManager.RandomStatHandlers.ContainsKey(statGenName)) {
                throw new ArgumentException($"Stat generator for rarity {rarity} not found");
            }

            RandomStatManager.CreateRandomStatsCard(statGenName, "Corrupted Card", "A random description", 1, 3, player);
        }
    }
}
