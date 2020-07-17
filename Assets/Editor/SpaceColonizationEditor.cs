using UnityEngine;
using UnityEditor;
using VoxelSystem;

[CustomEditor(typeof(SpaceColonization))]
public class SpaceColonizationEditor : Editor {

    private bool toggleLeafReferences = false;

    public override void OnInspectorGUI() {
        SpaceColonization spaceColonizer = (SpaceColonization)target;

        base.OnInspectorGUI();  

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate Tree", GUILayout.MaxWidth(150))) {
            Debug.Log("Generating Tree...");
            spaceColonizer.Generate();
        }

        if (GUILayout.Button("Voxelize Tree", GUILayout.MaxWidth(150))) {
            Debug.Log("Voxelizing...");
            MeshFilter voxelFilter = spaceColonizer.SetupVoxelize();

            var data = GPUVoxelizer.Voxelize(
                spaceColonizer.voxelizer, 
                spaceColonizer.treeObject.GetComponent<MeshFilter>().mesh, 
                spaceColonizer.SCData.resolution, 
                true);
			voxelFilter.sharedMesh = VoxelMesh.Build(data.GetData(), data.UnitLength, false);
			data.Dispose();

            Debug.Log("Finished Voxelation");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Toggle Active Tree", GUILayout.MaxWidth(303))) {
            spaceColonizer.ToggleActiveTree();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Toggle Leaves", GUILayout.MaxWidth(303))) {
            spaceColonizer.ToggleLeaves();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("SAVE", "Careful! Change the name before save or you might overwrite the previous version"))) {
            spaceColonizer.Save();
        }

        GUILayout.Space(32);

        if (GUILayout.Button(new GUIContent("CLEAR", "Careful! This will delete everything"))) {
            spaceColonizer.Clear();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
