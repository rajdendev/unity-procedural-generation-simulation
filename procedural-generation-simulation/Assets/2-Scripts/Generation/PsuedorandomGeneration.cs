using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PsuedorandomGeneration {
    private static Tiles[,] grid;
    private static MapSettings settings;
    private static System.Random prng;

    public static Tiles[,] Generate(MapSettings settings) {
        PsuedorandomGeneration.settings = settings;
        grid = new Tiles[settings.size, settings.size];

        if (settings.randomSeed) { settings.seed = System.DateTime.Now.Ticks.ToString(); }
        prng = new System.Random(settings.seed.GetHashCode());

        Randomize();

        return grid;
    }

    private static void Randomize() {
        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                grid[x, y].type = prng.Next(0, 100) < settings.pseudorandomSettings.randomFillPercent ? MapGenerator.land : MapGenerator.water;
            }
        }
    }
}
