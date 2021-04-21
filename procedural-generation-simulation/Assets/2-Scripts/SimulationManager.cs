using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationManager : MonoBehaviour {
    [SerializeField] private MapGenerator map = null;
    [SerializeField] private Pathfinding pathfinding = null;
    [SerializeField] private TextMeshProUGUI timeText = null;
    [SerializeField] private TextMeshProUGUI turnText = null;
    [SerializeField] private TextMeshProUGUI playerText = null;
    [SerializeField] private GameObject canvas = null;

    public MapGenerator Map { get { return map; } }
    public Pathfinding Pathfinding { get { return pathfinding; } }

    public GameObject playerPrefab, unitPrefab;
    public List<Player> players = new List<Player>();

    private int turn;
    private float time;

    private bool active;

    public static SimulationManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        canvas.gameObject.SetActive(true);
        map.Generate();

        foreach (Player player in players) {
            player.CreateUnit();
        }

        StartSimulation();
    }

    private void Update() {
        if (active) {
            time += Time.deltaTime;
            timeText.text = string.Format("TIME: {0:0.00}", time);
        }
    }

    private void StartSimulation() {
        active = true;
        turn = 1;
        time = 0;
        turnText.text = $"TURN: {turn}";
        playerText.text = $"PLAYERS: {players.Count}";
        NextTurn();
    }

    private void NextTurn() {
        //if (players.Count !> 0) { EndSimulation(); }
        foreach (Player player in players) {
            player.unit.Move();
        }

        turn++; 
        turnText.text = $"TURN: {turn}";
        playerText.text = $"PLAYERS: {players.Count}";
    }

    private void EndSimulation() {
        active = false;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public class Simulation {
        public int turns;
    }
}
