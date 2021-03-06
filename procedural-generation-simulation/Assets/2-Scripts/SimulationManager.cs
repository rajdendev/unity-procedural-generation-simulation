using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationManager : MonoBehaviour {
    [SerializeField, Min(1)] private int simulationAmount = 1000;
    private static int currentSimulation = 0;
    private static int currentGenerationType = 0;
    [SerializeField] private MapGenerator map = null;
    [SerializeField] private Pathfinding pathfinding = null;
    [SerializeField] private TextMeshProUGUI timeText = null;
    [SerializeField] private TextMeshProUGUI turnText = null;
    [SerializeField] private TextMeshProUGUI playerText = null;
    [SerializeField] private TextMeshProUGUI generationText = null;
    [SerializeField] private TextMeshProUGUI simulationText = null;
    [SerializeField] private GameObject canvas = null;

    public MapGenerator Map { get { return map; } }
    public Pathfinding Pathfinding { get { return pathfinding; } }

    public GameObject playerPrefab, unitPrefab;
    public List<Player> players = new List<Player>();

    private int turn;
    private float time;

    public bool active, nextTurn;

    public static SimulationManager instance;

    private const string path = @"Assets\simulation-results.xml";
    public static List<List<Simulation>> results = new List<List<Simulation>>();
    private static XmlSerializer serializer = new XmlSerializer(typeof(List<List<Simulation>>));

    private void Awake() {
        instance = this;
    }

    private void Start() {
        Init();
        map.Settings.generationType = (GenerationTypes)currentGenerationType;
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

        if (nextTurn) {
            NextTurn();
        }
    }

    private void StartSimulation() {
        active = true;
        nextTurn = true;
        turn = 1;
        time = 0;
        turnText.text = $"TURN: {turn}";
        playerText.text = $"PLAYERS: {players.Count}";
        generationText.text = ((GenerationTypes)currentGenerationType).ToString().ToUpper();
        simulationText.text = $"SIMULATION: {currentSimulation + 1}/{simulationAmount}";
    }

    private IEnumerator Turn() {
        nextTurn = false;
        foreach (Player player in players) {
            player.unit.Move();
            yield return new WaitForEndOfFrame();
        }

        turn++;
        turnText.text = $"TURN: {turn}";
        playerText.text = $"PLAYERS: {players.Count}";
        generationText.text = ((GenerationTypes)currentGenerationType).ToString().ToUpper();
        simulationText.text = $"SIMULATION: {currentSimulation + 1}/{simulationAmount}";
        nextTurn = true;
    }

    public void NextTurn() {
        StartCoroutine(Turn());
    }

    public void EndSimulation() {
        if (!active) { return; }
        active = false;
        SerializeResults();
        currentSimulation++;
        if (currentSimulation >= simulationAmount) {
            currentGenerationType++;
            currentSimulation = 0;
            if (currentGenerationType > Enum.GetNames(typeof(GenerationTypes)).Length - 1) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            map.Settings.generationType = (GenerationTypes)currentGenerationType;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Init() {
        try {
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                results = (List<List<Simulation>>)serializer.Deserialize(stream);
            }
        }
        catch { Debug.Log("NOPERS"); }

        if (results.Count != 3) {
            results = new List<List<Simulation>>() {
                new List<Simulation>(),
                new List<Simulation>(),
                new List<Simulation>()
            };
        }
    }

    private void SerializeResults() {
        results[(int)map.Settings.generationType].Add(new Simulation() { turns = turn, time = time });

        try {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate)) {
                serializer.Serialize(stream, results);
            }
        }
        catch { Debug.Log("Something went wrong"); }
    }
}

[System.Serializable]
public class Simulation {
    public int turns;
    public float time;
}
