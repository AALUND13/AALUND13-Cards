using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    public class GlitchingCardEffect : MonoBehaviour {
        private void Start() {
            if(gameObject.transform.childCount == 0) return;
            this.ExecuteAfterFrames(1, () => {

                GlitchingTextMono glitchingTextMono = gameObject.GetOrAddComponent<GlitchingTextMono>();
                glitchingTextMono.EffectAllText = false;

                TextMeshProUGUI[] statsText = gameObject.transform.GetChild(0).Find("Canvas/Front/Grid").GetComponentsInChildren<TextMeshProUGUI>();
                foreach(TextMeshProUGUI text in statsText) {
                    glitchingTextMono.AddTextMesh(text);
                }

                TextMeshProUGUI cardTitle = gameObject.transform.GetChild(0).Find("Canvas/Front/Text_Name").GetComponent<TextMeshProUGUI>();
                glitchingTextMono.AddTextMesh(cardTitle);
            });
        }
    }
}
