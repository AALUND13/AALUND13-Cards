using RarityLib.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AALUND13Cards.Core.Utils {
    public class RandomCardRarity : MonoBehaviour {
        public Image[] edges;

        public void OnEnable() {
            if(!Application.isEditor) {
                var rarity = GetRandomRarity();
                foreach(var edge in edges) {
                    edge.color = rarity.color;
                }
            } else {
                // In the editor, we just log and don't apply any rarity.
                // Because `RarityLib` or any mods is not initialized in the editor.
                UnityEngine.Debug.Log("RandomCardRarity is running in the editor");
            }
        }

        public static Rarity GetRandomRarity() {
            float allRaritiesWeight = RarityUtils.Rarities.Sum(r => r.Value.calculatedRarity);
            float randomValue = UnityEngine.Random.Range(0f, allRaritiesWeight);
            
            Rarity rarity = null;
            foreach(var rarityEntry in RarityUtils.Rarities) {
                randomValue -= rarityEntry.Value.calculatedRarity;
                if(randomValue <= 0f) {
                    rarity = rarityEntry.Value;
                    break;
                }
            }

            return rarity;
        }
    }
}
