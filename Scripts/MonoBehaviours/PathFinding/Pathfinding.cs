using AALUND13Card.MonoBehaviours.PathFinding;
using AALUND13Card.MonoBehaviours.PathFinding.Heap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PathRequestManager), typeof(GridManager))]
public class Pathfinding : MonoBehaviour {
    PathRequestManager pathRequestManager;
    GridManager gridManager;

    private void Awake() {
        pathRequestManager = GetComponent<PathRequestManager>();
        gridManager = GetComponent<GridManager>();
    }

    private IEnumerator FindPath(Vector2 start, Vector2 end) {
        Node startNode = gridManager.NodeFromWorldPoint(start);
        Node endNode = gridManager.NodeFromWorldPoint(end);

        Vector2[] path = new Vector2[0];
        bool pathSuccess = false;

        if(startNode == null || endNode == null || !startNode.walkable || !endNode.walkable) {
            yield return null;
            pathRequestManager.FinishedProcessingPath(path, pathSuccess);
            yield break;
        }

        foreach(var n in gridManager.AllNodes) {
            n.gCost = int.MaxValue;
            n.hCost = 0;
            n.parent = null;
        }

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

            foreach(var nbr in gridManager.GetNeighbors(current)) {
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

        // no path found
        yield return null;

        if(pathSuccess) {
            path = RetracePath(startNode, endNode);
        }
        pathRequestManager.FinishedProcessingPath(path, pathSuccess);
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

    public void StartFindPath(Vector2 startPos, Vector2 endPos) {
        StartCoroutine(FindPath(startPos, endPos));
    }
}