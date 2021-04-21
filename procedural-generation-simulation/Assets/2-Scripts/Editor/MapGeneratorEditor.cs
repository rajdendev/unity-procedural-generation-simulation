using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        MapGenerator mapGenerator = (MapGenerator)target;

        base.OnInspectorGUI();

        if (mapGenerator.Settings == null) { return; }
        Editor editor = CreateEditor(mapGenerator.Settings);
        editor.OnInspectorGUI();

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate")) {
            if (Application.isPlaying) {
                mapGenerator.Generate();
            }
        }
    }
}
