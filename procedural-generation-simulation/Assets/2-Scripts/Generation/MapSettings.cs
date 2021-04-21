using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Settings")]
public class MapSettings : ScriptableObject {
    [Header("General")]
    [Min(5)] public int size = 64;
    public bool randomSeed;
    public string seed;

    [Header("Generation")]
    public GenerationTypes generationType = GenerationTypes.Psuedorandom;
    public PseudorandomSettings pseudorandomSettings;
    public CellularAutomataSettings cellularAutomataSettings;
    public PerlinNoiseSettings perlinNoiseSettings;

    [Header("Player")]
    public Color[] playerColors = new Color[4];

    [System.Serializable]
    public struct PseudorandomSettings {
        [Range(0, 100)] public int randomFillPercent;
    }

    [System.Serializable]
    public struct CellularAutomataSettings {
        [Range(0, 10)] public int iterations;
        [Range(0, 100)] public int randomFillPercent;
    }

    [System.Serializable]
    public struct PerlinNoiseSettings {
        public float scale;
        public int octaves;
        [Range(0, 1)] public float persistance;
        public float lucunarity;
        public Vector2 offset;
    }
}
