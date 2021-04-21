using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoiseGeneration {
    private static Tiles[,] grid;
    private static MapSettings settings;
    private static System.Random prng;

    public static Tiles[,] Generate(MapSettings settings) {
        PerlinNoiseGeneration.settings = settings;
        grid = new Tiles[settings.size, settings.size];

        if (settings.randomSeed) { settings.seed = System.DateTime.Now.Ticks.ToString(); }
        prng = new System.Random(settings.seed.GetHashCode());

        Noise();

        return grid;
    }

    private static void Noise() {
        Vector2[] octaveOffsets = new Vector2[settings.perlinNoiseSettings.octaves];
        for (int i = 0; i < settings.perlinNoiseSettings.octaves; i++) {
            float offsetX = prng.Next(-10000, 10000) + settings.perlinNoiseSettings.offset.x;
            float offsetY = prng.Next(-10000, 10000) + settings.perlinNoiseSettings.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (settings.perlinNoiseSettings.scale <= 0) {
            settings.perlinNoiseSettings.scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = settings.size / 2f;

        for (int y = 0; y < settings.size; y++) {
            for (int x = 0; x < settings.size; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < settings.perlinNoiseSettings.octaves; i++) {
                    float sampleX = (x - halfWidth) / settings.perlinNoiseSettings.scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfWidth) / settings.perlinNoiseSettings.scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) / 2f;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= settings.perlinNoiseSettings.persistance;
                    frequency *= settings.perlinNoiseSettings.lucunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseHeight = Mathf.RoundToInt(noiseHeight);
                grid[x, y].type = (int)noiseHeight;
            }
        }
    }
}
