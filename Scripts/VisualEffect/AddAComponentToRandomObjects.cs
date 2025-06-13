using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.VisualEffect {
    public class AddAComponentToRandomObjects : MonoBehaviour {
        public int NumberOfObjectsToAdd = 1;
        public MonoBehaviour ComponentToAdd;
        public List<GameObject> TargetObjects;

        private void Start() {
            if(TargetObjects == null || TargetObjects.Count == 0) {
                Debug.LogWarning("No TargetObjects specified to add the component to.");
                return;
            }

            for(int i = 0; i < NumberOfObjectsToAdd; i++) {
                AddComponentToRandomObject();
            };
        }

        private void AddComponentToRandomObject() {
            if(TargetObjects == null || TargetObjects.Count == 0) {
                Debug.LogWarning("No TargetObjects specified to add the component to.");
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, TargetObjects.Count);
            GameObject targetObject = TargetObjects[randomIndex];
            if(targetObject.GetComponent(ComponentToAdd.GetType()) != null) {
                Debug.LogWarning($"Component {ComponentToAdd.GetType().Name} already exists on {targetObject.name}");
                return;
            }

            targetObject.AddComponent(ComponentToAdd.GetType());
            TargetObjects.RemoveAt(randomIndex);
        }
    }
}
