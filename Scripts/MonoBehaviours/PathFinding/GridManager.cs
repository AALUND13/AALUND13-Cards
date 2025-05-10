using UnityEngine;
using System.Collections.Generic;

namespace AALUND13Card.MonoBehaviours.PathFinding {
    public class GridManager : MonoBehaviour {
        [Header("Grid Settings")]
        public int gridWorldWidth = 20;
        public int gridWorldHeight = 15;
        public float nodeRadius = 0.5f;
        public int NodesBatchSize = 10;
        public LayerMask obstacleMask;

        [Header("Placement")]
        public Vector2 gridCenter = Vector2.zero;

        [Header("Debug")]
        public bool showGridGizmos = true;

        private Node[,] grid;
        private float nodeDiameter;
        private Vector2 bottomLeft;
        private int gridSizeX, gridSizeY;
        static readonly (int dx, int dy)[] NeighborsOffsets = {
      (-1,-1),(-1,0),(-1,1),(0,-1),(0,1),(1,-1),(1,0),(1,1)
    };


        private int gridIndex;
        private int NodesPerFrame => gridSizeX * gridSizeY / NodesBatchSize;


        public IEnumerable<Node> AllNodes {
            get {
                foreach(var n in grid) yield return n;
            }
        }
        public int NodeCount => gridWorldWidth * gridWorldHeight;

        void Awake() {
            nodeDiameter = nodeRadius * 2f;
            gridSizeX = Mathf.Max(1, Mathf.RoundToInt(gridWorldWidth / nodeDiameter));
            gridSizeY = Mathf.Max(1, Mathf.RoundToInt(gridWorldHeight / nodeDiameter));

            float realWidth = gridSizeX * nodeDiameter;
            float realHeight = gridSizeY * nodeDiameter;

            bottomLeft = gridCenter
                       - Vector2.right * (realWidth * 0.5f)
                       - Vector2.up * (realHeight * 0.5f);

            CreateGrid();
        }

        void Update() {
            int totalNodes = gridSizeX * gridSizeY;
            for(int i = 0; i < NodesPerFrame; i++) {
                gridIndex = (gridIndex + 1) % totalNodes;

                int x = gridIndex / gridSizeY;
                int y = gridIndex % gridSizeY;

                Node node = grid[x, y];
                node.walkable = !Physics2D.OverlapCircle(
                    node.worldPosition, nodeRadius, obstacleMask);
            }
        }

        void CreateGrid() {
            grid = new Node[gridSizeX, gridSizeY];
            for(int x = 0; x < gridSizeX; x++) {
                for(int y = 0; y < gridSizeY; y++) {
                    Vector2 wp = bottomLeft
                               + Vector2.right * (x * nodeDiameter + nodeRadius)
                               + Vector2.up * (y * nodeDiameter + nodeRadius);

                    List<int> neighborIndices = new List<int>(8);
                    for(int dx = -1; dx <= 1; dx++)
                        for(int dy = -1; dy <= 1; dy++) {
                            if(dx == 0 && dy == 0) continue;
                            int nx = x + dx, ny = y + dy;
                            if(nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY)
                                neighborIndices.Add(nx * gridSizeY + ny);
                        }

                    bool walkable = !Physics2D.OverlapCircle(wp, nodeRadius, obstacleMask);
                    grid[x, y] = new Node(wp, x, y, walkable, neighborIndices);
                }
            }
        }

        public void RefreshWalkability() {
            foreach(var n in grid) {
                n.walkable = !Physics2D.OverlapCircle(
                    n.worldPosition, nodeRadius, obstacleMask);
            }
        }

        public Node NodeFromWorldPoint(Node[,] grid, Vector2 worldPos) {
            float px = Mathf.Clamp01((worldPos.x - bottomLeft.x) / (gridSizeX * nodeDiameter));
            float py = Mathf.Clamp01((worldPos.y - bottomLeft.y) / (gridSizeY * nodeDiameter));
            int x = Mathf.RoundToInt((gridSizeX - 1) * px);
            int y = Mathf.RoundToInt((gridSizeY - 1) * py);
            return grid[x, y];
        }

        public List<Node> GetNeighbors(Node[,] grid, Node node) {
            var result = new List<Node>(8);
            for(int i = 0; i < NeighborsOffsets.Length; i++) {
                int nx = node.gridX + NeighborsOffsets[i].dx, ny = node.gridY + NeighborsOffsets[i].dy;
                if(nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY)
                    result.Add(grid[nx, ny]);
            }
            return result;
        }

        public Node[,] CloneGrid() {
            Node[,] clone = new Node[gridSizeX, gridSizeY];
            for(int x = 0; x < gridSizeX; x++) {
                for(int y = 0; y < gridSizeY; y++) {
                    clone[x, y] = new Node(grid[x, y]);
                }
            }
            return clone;
        }

        void OnDrawGizmos() {
            if(!showGridGizmos) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(
                gridCenter,
                new Vector3(gridSizeX * nodeDiameter, gridSizeY * nodeDiameter, 1));

            if(grid != null) {
                foreach(var n in grid) {
                    Gizmos.color = n.walkable ? Color.white : Color.red;
                    Gizmos.DrawCube(
                        n.worldPosition,
                        Vector3.one * (nodeDiameter * 0.9f));
                }
            }
        }
    }
}