using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceColonization : MonoBehaviour {
    private Tree tree;
    private GameObject treeObject;

    //public GameObject leafPrefab;
    public Material leafMat;

    public float width = 0.1f;
    public float length = 0.1f;

    [Header("Tree Attributes")]
    public float maxDist = 1f;
    public float minDist = .1f;
    public int numLeaves = 500;
    public int growIterations = 1;

    [Header("Mesh Generation")]
    public float invertedGrowth = 1.5f;
    public Material meshMaterial;
    [Range(3,10)]
    public int radialSubdivisions = 10;

    private Helper helper;

    private void Start() {
        helper = new Helper();
    }

    public void Generate() {
        //clean up old stuff
        helper.ClearAllChildren(this.transform);
        Destroy(treeObject);

        //new tree object
        treeObject = new GameObject();
        treeObject.name = "Tree";
        treeObject.transform.position = new Vector3(0, -200, 0);

        //set up mesh
        treeObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = treeObject.AddComponent<MeshRenderer>();
        renderer.material = meshMaterial;

        //set up tree
        tree = treeObject.AddComponent<Tree>();
       // tree.leafPrefab = leafPrefab;
        tree.leafMat = leafMat;
        tree.parent = this.transform;
        tree.width = width;
        tree.length = length;
        tree.minDist = minDist;
        tree.maxDist = maxDist;
        tree.numLeaves = numLeaves;
        tree.invertedGrowth = invertedGrowth;
        tree.radialSubdivisions = radialSubdivisions;

        tree.setup();

        for (int i = 0; i < growIterations; i++) {
            tree.grow();
        }

        tree.show();
    }
}
