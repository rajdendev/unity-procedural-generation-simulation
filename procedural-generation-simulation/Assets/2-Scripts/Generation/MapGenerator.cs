using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private TileType[] tileTypes = new TileType[2];
    [SerializeField] private MapSettings settings;
    public MapSettings Settings { get => settings; }

    public const int water = 0;
    public const int land = 1;

    private Tiles[,] grid;
    public Tiles[,] Grid { get => grid; }

    public int MaxSize {
        get {
            return settings.size * settings.size;
        }
    }

    public void Generate() {
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        grid = GenerateData();
        GenerateTiles();
    }

    private Tiles[,] GenerateData() {
        return settings.generationType switch {
            GenerationTypes.Psuedorandom => PsuedorandomGeneration.Generate(settings),
            GenerationTypes.CellularAutomata => CellularAutomataGeneration.Generate(settings),
            GenerationTypes.PerlinNoise => PerlinNoiseGeneration.Generate(settings),
            _ => grid,
        };
    }

    private void GenerateTiles() {
        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                GameObject tile = Instantiate(tileTypes[grid[x, y].type].prefab);
                tile.name = $"Tile[{x},{y}]";
                tile.transform.position = new Vector2(
                    x + -settings.size / 2f + tile.transform.localScale.x / 2f,
                    y + -settings.size / 2f + tile.transform.localScale.y / 2f);
                tile.transform.parent = transform;
                tile.GetComponent<SpriteRenderer>().color = tileTypes[grid[x, y].type].color;

                Tile t = tile.GetComponent<Tile>();
                t.gridPos = new Vector2Int(x, y);
                t.movementPenalty = tileTypes[grid[x, y].type].moveCost;
                grid[x, y].tile = t;
            }
        }

        GetAdjacents();
    }

    private void GetAdjacents() {
        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                grid[x, y].tile.adjacents = FindAdjacents(grid[x, y].tile.gridPos.x, grid[x, y].tile.gridPos.y);
            }
        }
    }

    private List<Tile> FindAdjacents(int tileX, int tileY) {
        List<Tile> adjacents = new List<Tile>();
        for (int y = tileY - 1; y <= tileY + 1; y++) {
            for (int x = tileX - 1; x <= tileX + 1; x++) {
                if (x == tileX && y == tileY) { continue; }
                if (!IsInMapRange(x, y)) { continue; }

                adjacents.Add(grid[x, y].tile);
            }
        }

        return adjacents;
    }

    private bool IsInMapRange(int x, int y) => x >= 0 && x < settings.size && y >= 0 && y < settings.size;

    public List<Tile> path;
    private void OnDrawGizmos() {
        if (grid == null || path == null) { return; }
        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                if (path.Contains(grid[x, y].tile)) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(grid[x, y].tile.transform.position, Vector2.one);
                }
            }
        }
    }
}

public struct Tiles {
    public int type;
    public Tile tile;
}

public enum GenerationTypes {
    Psuedorandom,
    CellularAutomata,
    PerlinNoise
}
