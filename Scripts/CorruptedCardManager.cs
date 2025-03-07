using AALUND13Card.Cards;
using AALUND13Card.RandomStatGenerators;
using JARL.Bases;
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

        public void GiveCorruptedCard(Player player, CardRarity cardRarity) {
            string statGenName = $"CorruptedStatGenerator_{cardRarity}";
            if(!RandomStatManager.RandomStatHandlers.ContainsKey(statGenName)) {
                throw new ArgumentException($"Stat generator for rarity {cardRarity} not found");
            }

            RandomStatManager.CreateRandomStatsCard(statGenName, "Corrupted Card", "A random description", 1, 3, player);
        }
    }
}
