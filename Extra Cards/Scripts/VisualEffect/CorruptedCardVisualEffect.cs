using UnityEngine;

namespace AALUND13Cards.Core.VisualEffect {
    public class CorruptedCardVisualEffect : MonoBehaviour {
        private const float ROTATE_CARD_CHANCE = 5f;
        private const float ROTATE_CARD_ANGLE_LIMIT = 180f;

        private float oldRotationZ;
        private bool init;

        private void Awake() {
            oldRotationZ = transform.rotation.eulerAngles.z;
            init = true;
        }

        private void Update() {
            if(!init) {
                oldRotationZ = transform.rotation.eulerAngles.z;
                init = true;
            }

            if(Random.Range(0f, 100f) < ROTATE_CARD_CHANCE) {
                transform.Rotate(0f, 0f, Random.Range(-ROTATE_CARD_ANGLE_LIMIT, ROTATE_CARD_ANGLE_LIMIT));
            } else {
                Vector3 currentRotation = transform.rotation.eulerAngles;
                currentRotation.z = oldRotationZ;
                transform.rotation = Quaternion.Euler(currentRotation);
            }
        }

        private void OnDisable() {
            if(!init) return;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.z = oldRotationZ;
            transform.rotation = Quaternion.Euler(currentRotation);
        }

        private void OnDestroy() {
            if(!init) return;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.z = oldRotationZ;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
}
