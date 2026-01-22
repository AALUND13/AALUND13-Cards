using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Classes.UI {
    public class UILoopingMover : MonoBehaviour {
        [Header("UI Elements")]
        [SerializeField] private List<RectTransform> uiElements;

        [Header("Movement")]
        [SerializeField] private RectTransform moveTarget;
        [SerializeField] private float moveSpeed = 200f;
        [SerializeField] private float reachThreshold = 5f;

        [Header("Teleport")]
        [SerializeField] private RectTransform teleportTarget;
        [SerializeField] private Vector2 teleportOffsetMin;
        [SerializeField] private Vector2 teleportOffsetMax;

        private void Update() {
            foreach(RectTransform element in uiElements) {
                MoveElement(element);
            }
        }

        private void MoveElement(RectTransform element) {
            element.anchoredPosition = Vector2.MoveTowards(
                element.anchoredPosition,
                moveTarget.anchoredPosition,
                moveSpeed * Time.deltaTime
            );

            if(Vector2.Distance(element.anchoredPosition, moveTarget.anchoredPosition) <= reachThreshold) {
                TeleportElement(element);
            }
        }

        private void TeleportElement(RectTransform element) {
            Vector2 randomOffset = new Vector2(
                Random.Range(teleportOffsetMin.x, teleportOffsetMax.x),
                Random.Range(teleportOffsetMin.y, teleportOffsetMax.y)
            );

            element.anchoredPosition = teleportTarget.anchoredPosition + randomOffset;
        }

        private void OnDrawGizmos() {
            if(teleportTarget == null) return;

            Gizmos.color = Color.cyan;

            Vector3 localBottomLeft = new Vector3(teleportOffsetMin.x, teleportOffsetMin.y, 0f);
            Vector3 localTopRight = new Vector3(teleportOffsetMax.x, teleportOffsetMax.y, 0f);

            Vector3 localTopLeft = new Vector3(
                teleportOffsetMin.x,
                teleportOffsetMax.y,
                0f
            );

            Vector3 localBottomRight = new Vector3(
                teleportOffsetMax.x,
                teleportOffsetMin.y,
                0f
            );

            Vector3 worldBottomLeft = teleportTarget.TransformPoint(localBottomLeft);
            Vector3 worldTopLeft = teleportTarget.TransformPoint(localTopLeft);
            Vector3 worldTopRight = teleportTarget.TransformPoint(localTopRight);
            Vector3 worldBottomRight = teleportTarget.TransformPoint(localBottomRight);

            Gizmos.DrawLine(worldBottomLeft, worldTopLeft);
            Gizmos.DrawLine(worldTopLeft, worldTopRight);
            Gizmos.DrawLine(worldTopRight, worldBottomRight);
            Gizmos.DrawLine(worldBottomRight, worldBottomLeft);
        }
    }
}
