using System.Collections.Generic;
using System.Threading;

namespace AALUND13Card.MonoBehaviours.PathFinding {
    internal class PathfindingThread {
        const int IDLE_SLEEP = 5;

        public Thread thread;
        public int threadId;
        public bool isRunning;

        private readonly Pathfinding pathfinding;
        private readonly Queue<PathRequest> pathfindingRequests = new Queue<PathRequest>();
        private readonly Queue<PathResult> pathfindingResults = new Queue<PathResult>();

        public PathfindingThread(int threadId, Pathfinding pathfinding) {
            this.threadId = threadId;
            this.pathfinding = pathfinding;
        }

        public void StartThread() {
            isRunning = true;
            thread = new Thread(new ParameterizedThreadStart(RunThread));
            thread.Start(threadId);
        }

        public void StopThread() {
            isRunning = false;
            thread.Join();
        }

        public void RunThread(object id) {
            int threadId = (int)id;
            
            while(isRunning) {
                if(pathfindingRequests.Count > 0) {
                    PathRequest request;
                    lock(pathfindingRequests) {
                        request = pathfindingRequests.Dequeue();
                    }

                    pathfinding.FindPath(request, FinishedProcessingPath);
                } else {
                    Thread.Sleep(IDLE_SLEEP);
                }
            }
        }

        public void AssignRequests(Queue<PathRequest> requests) {
            lock(pathfindingRequests) {
                while(requests.Count > 0) {
                    pathfindingRequests.Enqueue(requests.Dequeue());
                }
            }
        }

        public void FinishedProcessingPath(PathResult result) {
            lock(pathfindingResults) {
                pathfindingResults.Enqueue(result);
            }
        }

        public void ProcessResults() {
            if(pathfindingResults.Count > 0) {
                lock(pathfindingResults) {
                    for(int i = 0; i < pathfindingResults.Count; i++) {
                        PathResult result = pathfindingResults.Dequeue();
                        result.callback(result.path, result.success);
                    }
                }
            }
        }
    }
}
