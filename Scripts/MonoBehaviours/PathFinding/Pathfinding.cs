using AALUND13Card.MonoBehaviours.PathFinding.Heap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours.PathFinding {
    [RequireComponent(typeof(PathRequestManager), typeof(GridManager))]
    public class Pathfinding : MonoBehaviour {
        GridManager gridManager;

        private void Awake() {
            gridManager = GetComponent<GridManager>();
        }

        internal void FindPath(PathRequest request, Action<PathResult> callback) {
            Node[,] localGrid = gridManager.CloneGrid();
            Node startNode = gridManager.NodeFromWorldPoint(localGrid, request.startPos);
            Node endNode = gridManager.NodeFromWorldPoint(localGrid, request.endPos);

            Vector2[] path = new Vector2[0];
            bool pathSuccess = false;

            if(startNode != null && endNode != null && startNode.walkable && endNode.walkable) {
                var openHeap = new Heap<Node>(gridManager.NodeCount);
                var closedSet = new HashSet<Node>();

                startNode.gCost = 0;
                startNode.hCost = GetDistance(startNode, endNode);
                openHeap.Add(startNode);

                while(openHeap.Count > 0) {
                    Node current = openHeap.RemoveFirst();

                    if(current == endNode) {
                        pathSuccess = true;
                        break;
                    }

                    closedSet.Add(current);

                    foreach(var nbr in gridManager.GetNeighbors(localGrid, current)) {
                        if(!nbr.walkable || closedSet.Contains(nbr))
                            continue;

                        int tentativeG = current.gCost + GetDistance(current, nbr);
                        if(tentativeG < nbr.gCost) {
                            nbr.gCost = tentativeG;
                            nbr.hCost = GetDistance(nbr, endNode);
                            nbr.parent = current;

                            if(!openHeap.Contains(nbr))
                                openHeap.Add(nbr);        // O(log N)
                            else
                                openHeap.UpdateItem(nbr); // O(log N)
                        }
                    }
                }
            }

            if(pathSuccess) {
                path = RetracePath(startNode, endNode);
                pathSuccess = path.Length > 0;
            }

            callback(new PathResult(path, pathSuccess, request.callback));
        }

        private Vector2[] RetracePath(Node startNode, Node endNode) {
            var path = new List<Vector2>();
            Node curr = endNode;

            while(curr != startNode) {
                path.Add(curr.worldPosition);
                curr = curr.parent;
            }

            Vector2[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            return waypoints;
        }

        private Vector2[] SimplifyPath(List<Vector2> path) {
            List<Vector2> waypoints = new List<Vector2>();
            Vector2 prevDir = Vector2.zero;

            for(int i = 1; i < path.Count; i++) {
                Vector2 dir = (path[i] - path[i - 1]).normalized;
                if(dir != prevDir) {
                    waypoints.Add(path[i - 1]);
                    prevDir = dir;
                }
            }

            return waypoints.ToArray();
        }

        private int GetDistance(Node a, Node b) {
            int dx = Mathf.Abs(a.gridX - b.gridX);
            int dy = Mathf.Abs(a.gridY - b.gridY);
            if(dx > dy)
                return 14 * dy + 10 * (dx - dy);
            return 14 * dx + 10 * (dy - dx);
        }
    }
}