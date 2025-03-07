using AALUND13Card.MonoBehaviours;
using AALUND13Card.RandomStatGenerators;
using JARL.Bases;
using Photon.Pun;
using UnityEngine;

namespace AALUND13Card.Cards {
    public class CorruptedCard : MonoBehaviour, IPunInstantiateMagicCallback {
        public CardRarity Rarity;
        public Vector2Int RandomStatRange;

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            var data = info.photonView.InstantiationData;
            if(data == null) return;

            Vector3 localScale = (Vector3)data[0];
            int seed = (int)data[1];
            string rarity = (string)data[2];

            Vector2 vector2Data = (Vector2)data[3];
            Vector2Int randomStatRange = new Vector2Int((int)vector2Data.x, (int)vector2Data.y);

            gameObject.transform.localScale = localScale;

            string statGenName = $"CorruptedStatGenerator_{rarity}";
            LoggerUtils.LogInfo($"[CorruptedCard] Generating card with seed {seed} and rarity {rarity} using stat generator {statGenName}");

            RandomStatHandler randomStatHandler = RandomStatManager.RandomStatHandlers[statGenName];
            GenerateCard(randomStatHandler, seed, randomStatRange);
        }

        private void GenerateCard(RandomStatHandler randomStatHandler, int seed, Vector2Int randomStatRange) {
            CardInfo cardInfo = GetComponent<CardInfo>();
            System.Random random = new System.Random(seed);

            randomStatHandler.GenerateRandomStats(random, "Corrupted Card", "A random description", randomStatRange.x, randomStatRange.y, null, null, (card, outputRandom) => {
                LoggerUtils.LogInfo($"[CorruptedCard] Generated card with seed {seed} and rarity {Rarity} using stat generator {randomStatHandler.StatGenName}");

                CardInfoStat[] stats = card.cardStats;
                randomStatHandler.ApplyRandomStats(cardInfo, new System.Random(seed), "Corrupted Card", randomStatRange.x, randomStatRange.y);
                LoggerUtils.LogInfo($"[CorruptedCard] Applied stats to card with seed {seed} and rarity {Rarity} using stat generator {randomStatHandler.StatGenName}");

                cardInfo.sourceCard = card;
                cardInfo.cardStats = stats;
                cardInfo.gameObject.AddComponent<GlitchingCardEffect>();

                CardInfoDisplayer cardInfoDisplayer = cardInfo.GetComponentInChildren<CardInfoDisplayer>();
                cardInfoDisplayer.DrawCard(stats, "Corrupted Card", "A random description");
                LoggerUtils.LogInfo($"[CorruptedCard] Displayed card with seed {seed} and rarity {Rarity} using stat generator {randomStatHandler.StatGenName}");
            });
        }
    }
}
