using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour {
    private MapGenerator map;

    private void Start() {
        map = GetComponent<MapGenerator>();
    }

    public List<Tile> FindPath(Vector2Int startPos, Vector2Int targetPos) {
        List<Tile> path = new List<Tile>();
        bool pathSuccess = false;

        Tile startTile = map.Grid[startPos.x, startPos.y].tile;
        Tile targetTile = map.Grid[targetPos.x, targetPos.y].tile;

        Heap<Tile> openSet = new Heap<Tile>(map.MaxSize);
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0) {
            Tile currentTile = openSet.RemoveFirst();
            closedSet.Add(currentTile);

            if (currentTile == targetTile) {
                pathSuccess = true;
                break;
            }

            foreach (Tile neighbour in currentTile.adjacents) {
                if (closedSet.Contains(neighbour)) { continue; }

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour) + neighbour.movementPenalty;
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetTile);
                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        if (pathSuccess) {
            path = RetracePath(startTile, targetTile);
        }

        return path;
    }

    private List<Tile> RetracePath(Tile startTile, Tile endTile) {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile) {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Tile tileA, Tile tileB) {
        int dstX = Mathf.Abs(tileA.gridPos.x - tileB.gridPos.x);
        int dstY = Mathf.Abs(tileA.gridPos.y - tileB.gridPos.y);

        if (dstX > dstY) { return 14 * dstY + 10 * (dstX - dstY); }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
