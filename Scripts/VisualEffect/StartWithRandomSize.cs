using UnityEngine;

namespace AALUND13Cards.VisualEffect {
    [DisallowMultipleComponent]
    public class StartWithRandomSize : MonoBehaviour {
        public Vector2 sizeRange = new Vector2(0.75f, 1.25f);

        private void Start() {
            float randomSize = UnityEngine.Random.Range(sizeRange.x, sizeRange.y);
            transform.localScale = new Vector3(randomSize, randomSize, 1f);
        }
    }
}
