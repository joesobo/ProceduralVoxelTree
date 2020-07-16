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
            Debug.Log("Finished Tree Generation");
        }

        if (GUILayout.Button("Voxelize Tree", GUILayout.MaxWidth(150))) {
            Debug.Log("Voxelizing...");
            MeshFilter voxelFilter = spaceColonizer.SetupVoxelize();

            //TODO: option to set resolution and 2 bools
            var data = GPUVoxelizer.Voxelize(spaceColonizer.voxelizer, spaceColonizer.treeObject.GetComponent<MeshFilter>().mesh, 32, true);
			voxelFilter.sharedMesh = VoxelMesh.Build(data.GetData(), data.UnitLength, false);
			data.Dispose();

            Debug.Log("Finished Voxelation");
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
        if (GUILayout.Button("Save to File", GUILayout.MaxWidth(150))) {

        }

        if (GUILayout.Button("Clear Tree", GUILayout.MaxWidth(150))) {
            spaceColonizer.CleanUp();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
