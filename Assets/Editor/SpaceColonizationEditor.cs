using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpaceColonization))]
public class SpaceColonizationEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SpaceColonization spaceColonizer = (SpaceColonization)target;

        if (GUILayout.Button("Generate Tree")) {
            spaceColonizer.Generate();
        }

        if (GUILayout.Button("Voxelize Tree")) {

        }

        if (GUILayout.Button("Show Leaves")) {

        }

        if (GUILayout.Button("Save")) {

        }
    }
}
