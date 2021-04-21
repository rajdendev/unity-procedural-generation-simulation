using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IHeapItem<Tile> {
    public Vector2Int gridPos;
    public List<Tile> adjacents;
    public Tile parent;

    private int heapIndex;
    public int gCost;
    public int hCost;
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Tile tileToCompare) {
        int compare = fCost.CompareTo(tileToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(tileToCompare.hCost);
        }

        return -compare;
    }
}
