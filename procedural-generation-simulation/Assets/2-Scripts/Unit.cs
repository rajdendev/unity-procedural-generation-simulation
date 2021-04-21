using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Player team;
    public Player target;
    public Vector2Int gridPos;
    public List<Tile> path;
    public int movement = 2;
    private int remaining;

    public void Move() {
        remaining = movement;

        while (remaining > 0) {
            if (!(SimulationManager.instance.players.Count > 0)) { return; }
            if (target == null || path == null) {
                FindTarget();
            }
            //if (gridPos == target.gridPos && target != null) {
            //    SimulationManager.instance.players.Remove(target);
            //    continue;
            //}
            if (path == null || path.Count < 1) {
                path = new List<Tile>();
                path = SimulationManager.instance.Pathfinding.FindPath(gridPos, target.gridPos);
            }

            Tile next = path[0];
            transform.position = next.transform.position;
            gridPos = next.gridPos;
            path.Remove(next);

            remaining -= next.movementPenalty;
        }
    }

    public void FindTarget() {
        float distance = float.MaxValue;
        foreach (Player player in SimulationManager.instance.players) {
            if (player == team) { continue; }

            float curDistance = distance;
            if (Vector2.Distance(transform.position, player.transform.position) < curDistance) {
                distance = Vector2.Distance(transform.position, player.transform.position);
                target = player;
            }
        }

        path = new List<Tile>();
        path = SimulationManager.instance.Pathfinding.FindPath(gridPos, target.gridPos);
    }
}
