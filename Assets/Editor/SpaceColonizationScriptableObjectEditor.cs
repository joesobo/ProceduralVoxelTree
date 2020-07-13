using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpaceColonizationScriptableObject))]
public class SpaceColonizationScriptableObjectEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SpaceColonizationScriptableObject spaceColonizerData = (SpaceColonizationScriptableObject)target;

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate Leaf Colors", GUILayout.MaxWidth(305))) {
            spaceColonizerData.GenerateColors();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorUtility.SetDirty(spaceColonizerData);
    }
}
