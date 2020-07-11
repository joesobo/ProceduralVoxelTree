using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpaceColonization))]
public class SpaceColonizationEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SpaceColonization spaceColonizer = (SpaceColonization)target;

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Tree")) {
            spaceColonizer.Generate();
        }

        if (GUILayout.Button("Voxelize Tree")) {

        }
        GUILayout.EndHorizontal();
        GUILayout.Space(8);

        if (GUILayout.Button("Toggle Leaves")) {
            spaceColonizer.ToggleLeaves();
        }

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save to File")) {

        }

        if (GUILayout.Button("Clear Tree")) {

        }
        GUILayout.EndHorizontal();
    }
}
