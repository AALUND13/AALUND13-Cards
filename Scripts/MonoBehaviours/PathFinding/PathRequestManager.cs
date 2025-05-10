using System;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours.PathFinding {
    [RequireComponent(typeof(Pathfinding))]
    public class PathRequestManager : MonoBehaviour {
        [Tooltip("The number of threads to use for pathfinding.")]
        public int Threads = 2; // The number of threads to use

        private Queue<PathRequest> pathfindingRequests = new Queue<PathRequest>();

        private PathfindingThread[] pathfindingThreads;

        private static PathRequestManager instance;
        private Pathfinding pathfinding;

        private void Awake() {
            instance = this;
            pathfinding = GetComponent<Pathfinding>();
            StartThreads();
        }

        private void Update() {
            if(pathfindingRequests.Count > 0) {
                SplitRequests();
            }
            for(int i = 0; i < Threads; i++) {
                pathfindingThreads[i].ProcessResults();
            }
        }

        public static void RequestPath(PathRequest request) {
            lock(instance.pathfindingRequests) {
                instance.pathfindingRequests.Enqueue(request);
            }
        }


        public void StartThreads() {
            pathfindingThreads = new PathfindingThread[Threads];
            for(int i = 0; i < Threads; i++) {
                pathfindingThreads[i] = new PathfindingThread(i, pathfinding);
                pathfindingThreads[i].StartThread();
            }
        }

        public void SplitRequests() {
            lock(pathfindingRequests) {
                int requestsPerThread = Mathf.CeilToInt((float)pathfindingRequests.Count / Threads);
                for(int i = 0; i < Threads; i++) {
                    Queue<PathRequest> threadRequests = new Queue<PathRequest>();
                    for(int j = 0; j < requestsPerThread && pathfindingRequests.Count > 0; j++) {
                        threadRequests.Enqueue(pathfindingRequests.Dequeue());
                    }
                    pathfindingThreads[i].AssignRequests(threadRequests);
                }
            }
        }

        private void OnDestroy() {
            for(int i = 0; i < Threads; i++) {
                pathfindingThreads[i].StopThread();
            }
        }
    }

    public struct PathRequest {
        public Vector2 startPos;
        public Vector2 endPos;

        public Action<Vector2[], bool> callback;
        public PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> callback) {
            this.startPos = start;
            this.endPos = end;
            this.callback = callback;
        }
    }

    public struct PathResult {
        public Vector2[] path;
        public bool success;
        public Action<Vector2[], bool> callback;
        public PathResult(Vector2[] path, bool success, Action<Vector2[], bool> callback) {
            this.path = path;
            this.success = success;
            this.callback = callback;
        }
    }

}
