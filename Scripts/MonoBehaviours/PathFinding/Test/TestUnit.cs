using UnityEngine;

namespace AALUND13Card.MonoBehaviours.PathFinding.Test {
    internal class TestUnit : MonoBehaviour {
        public Transform target;

        private Vector2[] path;

        private void Start() {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }

        private void OnPathFound(Vector2[] newPath, bool success) {
            if(success) {
                path = newPath;
                UnityEngine.Debug.Log("Path found!");
            } else {
                path = null;
                UnityEngine.Debug.Log("Path not found!");
            }

            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }

        private void OnDrawGizmos() {
            if(path != null) {
                Gizmos.color = Color.green;
                for(int i = 0; i < path.Length; i++) {
                    Gizmos.DrawWireSphere(path[i], 1f);
                    if(i > 0) {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, path[0]);
            }
        }
    }
}
