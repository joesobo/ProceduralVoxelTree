using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceColonization : MonoBehaviour {
    private Tree tree;
    [HideInInspector]
    public GameObject treeObject;
    [HideInInspector]
    GameObject VoxelTree;

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
                tree.show();
                isShown = true;
                startGen = false;
            }
        }
    }

    public void Generate() {
        CleanUp();

        //new tree object
        treeObject = new GameObject();
        treeObject.name = "Tree";
        treeObject.transform.position = new Vector3(0, -200, 0);

        //set up mesh
        treeObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = treeObject.AddComponent<MeshRenderer>();
        meshMaterial.color = SCData.branchColor;
        renderer.material = meshMaterial;

        //set up tree
        tree = treeObject.AddComponent<Tree>();
        tree.SCData = SCData;
        tree.parent = this.transform;

        tree.setup();

        startGen = true;
        isGenerating = true;
    }

    public void CleanUp() {
        //reset timeout
        curTimeoutTime = 0;
        startGen = false;
        isGenerating = false;
        isShown = false;

        //clean up old stuff
        helper.ClearAllChildren(this.transform);
        Destroy(treeObject);
    }

    public void CleanUpVoxel() {
        Destroy(VoxelTree);
    }

    public void ToggleLeaves() {
        tree.toggleLeaves();
    }

    public void ToggleShowLeafReferences() {
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }

    public MeshFilter SetupVoxelize() {
        CleanUpVoxel();

        VoxelTree = new GameObject();
        VoxelTree.name = "Voxel Tree";
        VoxelTree.transform.position = SCData.rootPos;
        MeshRenderer renderer = VoxelTree.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = SCData.branchColor;
        MeshFilter filter = VoxelTree.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();

        return filter;
    }
}
