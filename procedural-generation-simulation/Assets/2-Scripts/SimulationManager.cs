using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationManager : MonoBehaviour {
    [SerializeField] private MapGenerator map = null;
    [SerializeField] private TextMeshProUGUI timeText = null;
    [SerializeField] private TextMeshProUGUI turnText = null;
    [SerializeField] private GameObject canvas = null;

    public GameObject playerPrefab;

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
        UpdateText(turn);
    }

    private void NextTurn() {
        turn++;
        UpdateText(turn);


    }

    private void EndSimulation() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateText(int turn) {
        turnText.text = $"TURN: {turn}";
    }

    public class Simulation {
        public int turns;
    }
}
