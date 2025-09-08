using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib.Utils;
using UnityEngine;

namespace AALUND13Cards.VisualEffect {
    public class AddRandomCardArt : MonoBehaviour {
        public bool AddRandomArtAtStart = true;
        public bool AddRandomArtAtEnable = false;
        public GameObject ArtParent;

        private void Start() {
            if(AddRandomArtAtStart) {
                GenerateRandomArt();
            }
        }

        private void OnEnable() {
            if(AddRandomArtAtEnable) {
                GenerateRandomArt();
            }
        }

        private void OnDisable() {
            foreach(Transform child in ArtParent.transform) {
                Destroy(child.gameObject);
            }
        }

        public void GenerateRandomArt() {
            if(Application.isEditor) {
                UnityEngine.Debug.LogWarning("Random Card Art cannot be generated in editor mode.");
                return;
            }

            CardInfo[] allCardsWithArts = CardManager.cards.Where(c => c.Value.cardInfo.cardArt != null).Select(c => c.Value.cardInfo).ToArray();
            if(allCardsWithArts.Length == 0) return;

            CardInfo randomCard = allCardsWithArts[UnityEngine.Random.Range(0, allCardsWithArts.Length)];
            GameObject art = UnityEngine.Object.Instantiate(randomCard.cardArt, ArtParent.transform);
            
            art.transform.localScale = Vector3.one;
            art.transform.localPosition = Vector3.zero;
        }
    }
}
