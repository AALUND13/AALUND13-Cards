using UnityEngine;
using UnityEngine.UI;

namespace AALUND13Cards.MonoBehaviours.UI {
    public class BarHandler : MonoBehaviour {
        public float BarValue = 0f;
        public float MaxBarValue = 100f;

        public Image BarFill;
        public Image BarStroke;

        private float strokeBarValue = 0f;

        private void Start() {
            if(BarFill != null) {
                BarFill.fillAmount = Mathf.Clamp01(BarValue / MaxBarValue);
            }
            if(BarStroke != null) {
                BarStroke.fillAmount = Mathf.Clamp01(strokeBarValue / MaxBarValue);
            }
        }

        private void Update() {
            if(BarFill != null) {
                BarFill.fillAmount = Mathf.Clamp01(BarValue / MaxBarValue);
            }
            if(BarStroke != null) {
                BarStroke.fillAmount = Mathf.Clamp01(strokeBarValue / MaxBarValue);
            }
            strokeBarValue = Mathf.Lerp(strokeBarValue, BarValue, Time.deltaTime * 5f);
        }
    }
}
