using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using VoxelSystem;

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
    private SaveHelper saveHelper;

    public ComputeShader voxelizer;

    public string saveName = "Tree";

    private void Start() {
        helper = new Helper();
        saveHelper = new SaveHelper();
        meshMaterial = new Material(Shader.Find("Standard"));

        meshFilter = GetComponent<MeshFilter>();
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
                setScale();
            }
        }
    }

    public void generate() {
        cleanUp();
        cleanUpVoxel();

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

    public void setScale() {
        treeObject.transform.localScale = SCData.scaleFactor * Vector3.one;
        treeObject.transform.position *= SCData.scaleFactor;
        treeLeaves.transform.localScale = SCData.scaleFactor * Vector3.one;
    }

    public void save() {
        //create save directory
        Directory.CreateDirectory("Assets/ProceduralVoxelTree/" + saveName + "/Meshes/Leaves");
        Directory.CreateDirectory("Assets/ProceduralVoxelTree/" + saveName + "/Materials/Leaves");
        Directory.CreateDirectory("Assets/ProceduralVoxelTree/" + saveName + "/Prefabs/Leaves");
        AssetDatabase.Refresh();

        saveHelper.saveMeshToDir(saveName, treeObject, voxelTree, treeLeaves);
        saveHelper.saveMatToDir(saveName, treeObject, voxelTree, treeLeaves);
        saveHelper.savePrefabToDir(saveName, treeObject, voxelTree, treeLeaves);
    }

    public void clear() {
        cleanUp();
        cleanUpVoxel();
    }

    private void cleanUp() {
        //reset timeout
        curTimeoutTime = 0;
        startGen = false;
        isGenerating = false;
        isShown = false;

        //clean up old stuff
        helper.clearAllChildren(this.transform);
        Destroy(treeObject);

        Destroy(treeLeaves);
    }

    private void cleanUpVoxel() {
        Destroy(voxelTree);
    }

    public void toggleLeaves() {
        foreach (Transform child in treeLeaves.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }

    public void toggleActiveTree() {
        if (treeObject) {
            treeObject.SetActive(!treeObject.activeSelf);
        }

        if (voxelTree) {
            voxelTree.SetActive(!voxelTree.activeSelf);
        }
    }

    public void toggleShowLeafReferences() {
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }

    public void setupVoxelize() {
        cleanUpVoxel();

        treeObject.SetActive(false);

        voxelTree = new GameObject();
        voxelTree.name = "Voxel Tree";
        voxelTree.transform.position = SCData.rootPos * SCData.scaleFactor;
        voxelTree.transform.localScale = SCData.scaleFactor * Vector3.one;
        MeshRenderer renderer = voxelTree.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = SCData.branchColor;
        MeshFilter filter = voxelTree.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();

        var data = GPUVoxelizer.Voxelize(
               voxelizer,
               treeObject.GetComponent<MeshFilter>().mesh,
               SCData.resolution,
               true);
        filter.sharedMesh = VoxelMesh.Build(data.GetData(), data.UnitLength, false);
        data.Dispose();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0, 0, 0.75f, 0.5f);
        if (SCData.leafRefShape == SpaceColonizationScriptableObject.LeafRefShape.Sphere) {
            Gizmos.DrawSphere(SCData.leafRefOffset, SCData.leafRefSpawnSize);
        } else {
            Gizmos.DrawCube(Vector3.zero, Vector3.one * SCData.leafRefSpawnSize * 2);
        }
    }
}
