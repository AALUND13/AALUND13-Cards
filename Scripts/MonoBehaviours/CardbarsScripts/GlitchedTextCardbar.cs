using AALUND13Card.MonoBehaviours;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours.CardbarsScripts {
    [RequireComponent(typeof(EnableTextFromCardbar))]
    public class GlitchedTextCardbar : MonoBehaviour {
        public void Start() {
            this.ExecuteAfterFrames(1, () => {
                EnableTextFromCardbar enableTextFromCardbar = GetComponent<EnableTextFromCardbar>();
                enableTextFromCardbar.text.gameObject.AddComponent<GlitchingTextMono>();
            });
        }
    }
}
