using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {
    public string name;
    public GameObject prefab;
    public Color color = Color.white;
    public int moveCost = 1;
}
