using AALUND13Card.Cards;
using Photon.Pun;
using RarityLib.Utils;
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
    }
}
