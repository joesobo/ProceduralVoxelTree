using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveHelper : MonoBehaviour {
    public void saveMeshToDir(string saveName, GameObject treeObject, GameObject voxelTree, GameObject treeLeaves) {
        MeshFilter treeFilter = treeObject.GetComponent<MeshFilter>();
        if (treeFilter) {
            // Assets/ProceduralVoxelTree/[Name]/Meshes/[Name].asset
            saveAsset("/Meshes/" + saveName + ".asset", saveName, treeFilter.mesh);
        }

        if (voxelTree) {
            MeshFilter voxelFilter = voxelTree.GetComponent<MeshFilter>();

            if (voxelFilter) {
                // Assets/ProceduralVoxelTree/[Name]/Meshes/[Name]Voxel.asset
                saveAsset("/Meshes/" + saveName + "Voxel.asset", saveName, voxelFilter.mesh);
            }
        }

        int count = 0;
        foreach (Transform child in treeLeaves.transform) {
            MeshFilter leaves = child.gameObject.GetComponent<MeshFilter>();

            if (leaves) {
                // Assets/ProceduralVoxelTree/[Name]/Meshes/Leaves/leaves0.asset
                saveAsset("/Meshes/Leaves/leaves" + count + ".asset", saveName, leaves.mesh);
                count++;
            }
        }
    }

    public void saveMatToDir(string saveName, GameObject treeObject, GameObject voxelTree, GameObject treeLeaves) {
        MeshRenderer treeRenderer = treeObject.GetComponent<MeshRenderer>();
        if (treeRenderer) {
            // Assets/ProceduralVoxelTree/[Name]/Materials/[Name].mat
            saveAsset("/Materials/" + saveName + ".mat", saveName, treeRenderer.material);
        }

        if (voxelTree) {
            MeshRenderer voxelRenderer = voxelTree.GetComponent<MeshRenderer>();

            if (voxelRenderer) {
                // Assets/ProceduralVoxelTree/[Name]/Materials/[Name]Voxel.mat
                saveAsset("/Materials/" + saveName + "Voxel.mat", saveName, voxelRenderer.material);
            }
        }

        int count = 0;
        foreach (Transform child in treeLeaves.transform) {
            MeshRenderer leavesRenderer = child.gameObject.GetComponent<MeshRenderer>();

            if (leavesRenderer) {
                // Assets/ProceduralVoxelTree/[Name]/Materials/Leaves/leaves0.mat
                saveAsset("/Materials/Leaves/leaves" + count + ".mat", saveName, leavesRenderer.material);
                count++;
            }
        }
    }

    public void savePrefabToDir(string saveName, GameObject treeObject, GameObject voxelTree, GameObject treeLeaves) {
        // Assets/ProceduralVoxelTree/[Name]/Prefabs/[Name].prefab
        savePrefab("/Prefabs/" + saveName + ".prefab", saveName, treeObject);

        // Assets/ProceduralVoxelTree/[Name]/Prefabs/[Name]Voxel.prefab
        if (voxelTree) {
            savePrefab("/Prefabs/" + saveName + "Voxel.prefab", saveName, voxelTree);
        }

        // Assets/ProceduralVoxelTree/[Name]/Prefabs/Leaves/[Name].prefab
        savePrefab("/Prefabs/Leaves/leaves.prefab", saveName, treeLeaves);
    }

    private void saveAsset(string path, string saveName, Object asset) {
        var savePath = "Assets/ProceduralVoxelTree/" + saveName + path;
        AssetDatabase.CreateAsset(asset, savePath);
        Debug.Log("Saved Asset to: " + savePath);
    }

    private void savePrefab(string path, string saveName, GameObject asset) {
        var savePath = "Assets/ProceduralVoxelTree/" + saveName + path;
        if (!asset.activeSelf) {
            asset.SetActive(true);
            PrefabUtility.SaveAsPrefabAsset(asset, savePath);
            asset.SetActive(false);
        } else {
            PrefabUtility.SaveAsPrefabAsset(asset, savePath);
        }
        Debug.Log("Saved Prefab to: " + savePath);
    }

}