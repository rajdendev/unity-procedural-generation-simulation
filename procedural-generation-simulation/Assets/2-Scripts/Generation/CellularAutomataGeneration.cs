using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellularAutomataGeneration {
    private static Tiles[,] grid;
    private static MapSettings settings;
    private static System.Random prng;

    public static Tiles[,] Generate(MapSettings settings) {
        CellularAutomataGeneration.settings = settings;
        grid = new Tiles[settings.size, settings.size];

        if (settings.randomSeed) { settings.seed = System.DateTime.Now.Ticks.ToString(); }
        prng = new System.Random(settings.seed.GetHashCode());

        Randomize();

        for (int i = 0; i < settings.cellularAutomataSettings.iterations; i++) {
            grid = Smoothen(grid);
        }

        return grid;
    }

    private static void Randomize() {
        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                grid[x, y].type = prng.Next(0, 100) < settings.cellularAutomataSettings.randomFillPercent ? MapGenerator.land : MapGenerator.water;
            }
        }
    }

    private static Tiles[,] Smoothen(Tiles[,] oldMap) {
        Tiles[,] newMap = new Tiles[settings.size, settings.size];

        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                int neighbourWaterTileCount = GetSurroundingTileCount(x, y, MapGenerator.water);

                if (oldMap[x, y].type == MapGenerator.water) {
                    if (neighbourWaterTileCount < 4) { newMap[x, y].type = MapGenerator.land; }
                    else { newMap[x, y].type = MapGenerator.water; }
                }
                else if (oldMap[x, y].type == MapGenerator.land) {
                    if (neighbourWaterTileCount > 4) { newMap[x, y].type = MapGenerator.water; }
                    else { newMap[x, y].type = MapGenerator.land; }
                }
            }
        }

        return newMap;
    }

    private static int GetSurroundingTileCount(int x, int y, int type, int depth = 1) {
        int tileCount = 0;

        for (int neighbourY = y - depth; neighbourY <= y + depth; neighbourY++) {
            for (int neighbourX = x - depth; neighbourX <= x + depth; neighbourX++) {
                if (IsInMapRange(neighbourX, neighbourY)) {
                    if (neighbourX != x || neighbourY != y) {
                        if (grid[neighbourX, neighbourY].type == type) {
                            tileCount++;
                        }
                    }
                }
                else {
                    tileCount++;
                }
            }
        }

        return tileCount;
    }

    private static bool IsInMapRange(int x, int y) => x >= 0 && x < settings.size && y >= 0 && y < settings.size;
}
