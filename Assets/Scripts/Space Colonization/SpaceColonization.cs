﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpaceColonization : MonoBehaviour {
    private Tree tree;
    [HideInInspector]
    public GameObject treeObject;
    [HideInInspector]
    public GameObject voxelTree;
    [HideInInspector]
    public GameObject treeLeaves;

    public SpaceColonizationScriptableObject SCData;

    public bool showLeafRef = false;

    private bool startGen = false;
    private bool isGenerating = false;
    private bool isShown = false;
    private float curTimeoutTime = 0;

    private Material meshMaterial;

    [HideInInspector]
    public MeshFilter meshFilter;

    private Helper helper;

    [HideInInspector]
    public SaveMeshInEditor meshSaver;
    public ComputeShader voxelizer;

    public string saveName = "Tree";

    private void Start() {
        helper = new Helper();
        meshMaterial = new Material(Shader.Find("Standard"));

        meshFilter = GetComponent<MeshFilter>();

        meshSaver = FindObjectOfType<SaveMeshInEditor>();
    }

    void Update() {
        if (startGen) {
            SCData.showLeafRef = showLeafRef;
            if (isGenerating) {
                curTimeoutTime += Time.deltaTime;

                if (curTimeoutTime < SCData.maxTimeoutTime) {
                    tree.grow();
                } else {
                    isGenerating = false;
                }
            }

            if (!isGenerating && !isShown) {
                tree.cleanLeaves();
                tree.show();
                isShown = true;
                startGen = false;
                Debug.Log("Finished Tree Generation");
            }
        }
    }

    public void Generate() {
        CleanUp();
        CleanUpVoxel();

        //new leaves object
        treeLeaves = new GameObject();
        treeLeaves.name = "Tree Leaves";

        //new tree object
        treeObject = new GameObject();
        treeObject.name = "Tree";
        treeObject.transform.position = SCData.rootPos;

        //set up mesh
        treeObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = treeObject.AddComponent<MeshRenderer>();
        meshMaterial.color = SCData.branchColor;
        renderer.material = meshMaterial;

        //set up tree
        tree = treeObject.AddComponent<Tree>();
        tree.SCData = SCData;
        tree.parent = this;

        tree.setup();

        startGen = true;
        isGenerating = true;
    }

    public void Clear() {
        CleanUp();
        CleanUpVoxel();
    }

    public void Save() {
        //Tree
        MeshFilter treeFilter = treeObject.GetComponent<MeshFilter>();

        if (treeFilter) {
            //create save directory
            Directory.CreateDirectory("Assets/Meshes/" + saveName + "/Leaves");
            Directory.CreateDirectory("Assets/Meshes/" + saveName + "/Materials/Leaves");
            AssetDatabase.Refresh();

            //  Assets/Meshes/Tree/Tree.asset
            var savePath = "Assets/Meshes/" + saveName + "/" + saveName + ".asset";
            AssetDatabase.CreateAsset(treeFilter.mesh, savePath);
            Debug.Log("Saved Mesh to: " + savePath);
        }

        //Voxel Tree
        if (voxelTree) {
            MeshFilter voxelFilter = voxelTree.GetComponent<MeshFilter>();

            if (voxelFilter) {
                //  Assets/Meshes/Tree/TreeVoxel.asset
                var savePath = "Assets/Meshes/" + saveName + "/" + saveName + "Voxel" + ".asset";
                AssetDatabase.CreateAsset(voxelFilter.mesh, savePath);
                Debug.Log("Saved Mesh to: " + savePath);
            }
        }

        //Leaves
        int count = 0;
        foreach (Transform child in treeLeaves.transform) {
            MeshFilter leaves = child.gameObject.GetComponent<MeshFilter>();

            if (leaves) {
                //  Assets/Meshes/Tree/leaves0.asset
                var savePath = "Assets/Meshes/" + saveName + "/Leaves/leaves" + count + ".asset";
                AssetDatabase.CreateAsset(leaves.mesh, savePath);
                Debug.Log("Saved Mesh to: " + savePath);
                count++;
            }
        }
    }

    private void CleanUp() {
        //reset timeout
        curTimeoutTime = 0;
        startGen = false;
        isGenerating = false;
        isShown = false;

        //clean up old stuff
        helper.ClearAllChildren(this.transform);
        Destroy(treeObject);

        Destroy(treeLeaves);
    }

    private void CleanUpVoxel() {
        Destroy(voxelTree);
    }

    public void ToggleLeaves() {
        foreach (Transform child in treeLeaves.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }

    public void ToggleActiveTree() {
        if (treeObject) {
            treeObject.SetActive(!treeObject.activeSelf);
        }

        if (voxelTree) {
            voxelTree.SetActive(!voxelTree.activeSelf);
        }
    }

    public void ToggleShowLeafReferences() {
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }

    public MeshFilter SetupVoxelize() {
        CleanUpVoxel();

        treeObject.SetActive(false);

        voxelTree = new GameObject();
        voxelTree.name = "Voxel Tree";
        voxelTree.transform.position = SCData.rootPos;
        MeshRenderer renderer = voxelTree.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = SCData.branchColor;
        MeshFilter filter = voxelTree.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();

        return filter;
    }
}
