using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours.CardbarsScripts {
    public class EnableTextFromCardbar : MonoBehaviour {
        public TextMeshProUGUI text;
        void OnEnable() {
            text = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
            this.ExecuteAfterFrames(1, () => {
                text.enabled = true;
            });
        }
    }
}
