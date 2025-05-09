using AALUND13Card.MonoBehaviours.PathFinding.Heap;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    public Vector2 worldPosition;
    public int gridX, gridY;
    public bool walkable;
    public int[] neighborIndices;

    public int gCost, hCost;
    public Node parent;
    public int fCost => gCost + hCost;

    int heapIndex;
    public int HeapIndex {
        get => heapIndex;
        set => heapIndex = value;
    }

    public Node(Vector2 worldPos, int x, int y, bool walkable, List<int> neighborIndices) {
        this.worldPosition = worldPos;
        this.gridX = x;
        this.gridY = y;
        this.walkable = walkable;
        this.neighborIndices = neighborIndices.ToArray();
    }

    public int CompareTo(Node other) {
        int cmp = fCost.CompareTo(other.fCost);
        if(cmp == 0)
            cmp = hCost.CompareTo(other.hCost);
        return cmp;
    }
}
