using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Core.VisualEffect {
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class StretchImageBetween : MonoBehaviour {
        public RectTransform pointA;
        public RectTransform pointB;

        private RectTransform rectTransform;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            if(pointA == null || pointB == null) return;

            Vector3 localA = rectTransform.parent.InverseTransformPoint(pointA.position);
            Vector3 localB = rectTransform.parent.InverseTransformPoint(pointB.position);

            Vector3 midPoint = (localA + localB) / 2f;
            rectTransform.localPosition = midPoint;

            Vector3 dir = localB - localA;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

            float distance = dir.magnitude;

            // Compensate for scaling so the length is visually correct
            float correctedWidth = distance / rectTransform.localScale.x;
            rectTransform.sizeDelta = new Vector2(correctedWidth, rectTransform.sizeDelta.y);
        }
    }
}
