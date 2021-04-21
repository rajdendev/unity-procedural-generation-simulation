using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Vector2Int gridPos;
    public Unit unit;

    private static int index;

    public void CreateUnit() {
        GameObject unitgo = Instantiate(SimulationManager.instance.unitPrefab);
        unitgo.name = $"Unit[{++index}]";
        unitgo.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        unit = unitgo.GetComponent<Unit>();
        unit.team = this;
        List<Tile> adjacents = SimulationManager.instance.Map.Grid[gridPos.x, gridPos.y].tile.adjacents;
        Tile tile = adjacents[Random.Range(0, adjacents.Count)];
        unit.gridPos = tile.gridPos;
        unitgo.transform.position = tile.transform.position;
    }
}
