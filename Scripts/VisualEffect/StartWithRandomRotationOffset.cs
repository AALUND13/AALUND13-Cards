using UnityEngine;

namespace AALUND13Cards.VisualEffect {
    public class StartWithRandomRotationOffset : MonoBehaviour {
        [Tooltip("The maximum rotation offset in degrees that will be applied to the object at start.")]
        public float rotationOffset = 25f;

        private void Start() {
            float randomOffset = UnityEngine.Random.Range(-rotationOffset, rotationOffset);

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.z += randomOffset;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
}
