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

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Unit")) {
            if(other.gameObject.GetComponent<Unit>() == unit) { return; }
            print(other.gameObject.name + " eliminated " + name);
            SimulationManager.instance.players.Remove(this);
            Destroy(unit.gameObject);
            Destroy(gameObject);

            if(SimulationManager.instance.players.Count <= 1) {
                SimulationManager.instance.EndSimulation();
            }

            foreach (Player player in SimulationManager.instance.players) {
                player.unit.FindTarget();
            }

            SimulationManager.instance.nextTurn = true;
        }
    }
}
