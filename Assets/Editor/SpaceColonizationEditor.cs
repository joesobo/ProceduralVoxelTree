using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpaceColonization))]
public class SpaceColonizationEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SpaceColonization spaceColonizer = (SpaceColonization)target;

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Tree", GUILayout.MaxWidth(150))) {
                spaceColonizer.Generate();
            }

            if (GUILayout.Button("Voxelize Tree", GUILayout.MaxWidth(150))) {

            }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
            if (GUILayout.Button("Toggle Leaves", GUILayout.MaxWidth(150))) {
                spaceColonizer.ToggleLeaves();
            }

            if (GUILayout.Button("Toggle Leaf References", GUILayout.MaxWidth(150))) {
                spaceColonizer.ToggleShowLeafReferences();
            }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save to File", GUILayout.MaxWidth(150))) {

            }

            if (GUILayout.Button("Clear Tree", GUILayout.MaxWidth(150))) {

            }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
