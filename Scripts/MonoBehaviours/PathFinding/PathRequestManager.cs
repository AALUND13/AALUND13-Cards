using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours.PathFinding {
    [RequireComponent(typeof(Pathfinding))]
    public class PathRequestManager : MonoBehaviour {
        private static PathRequestManager instance;
        private Pathfinding pathfinding;

        private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        private PathRequest currentPathRequest;

        private bool isProcessingPath;

        private void Awake() {
            instance = this;
            pathfinding = GetComponent<Pathfinding>();
        }

        public static void RequestPath(Vector2 startPos, Vector2 endPos, Action<Vector2[], bool> callback) {
            PathRequest newRequest = new PathRequest(startPos, endPos, callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        private void TryProcessNext() {
            if(!isProcessingPath && pathRequestQueue.Count > 0) {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.startPos, currentPathRequest.endPos);
            }
        }

        public void FinishedProcessingPath(Vector2[] path, bool success) {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest {
            public Vector2 startPos;
            public Vector2 endPos;
            public Action<Vector2[], bool> callback;
            public PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> callback) {
                this.startPos = start;
                this.endPos = end;
                this.callback = callback;
            }
        }
    }
}
