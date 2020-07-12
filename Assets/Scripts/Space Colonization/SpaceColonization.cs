using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceColonization : MonoBehaviour {
    private Tree tree;
    private GameObject treeObject;

    [Header("Leaf Attributes")]
    public Color leafColor1 = Color.white;
    public Color leafColor2 = Color.white;

    [Header("Branch Attributes")]
    public Vector2 width = new Vector2(0.1f, 0.2f);
    public Vector2 length = new Vector2(0.1f, 0.2f);
    public Color branchColor = Color.white;

    [Header("Space Colonization Attributes")]
    public Vector3 rootPos = new Vector3(0, -200, 0);
    public int numLeaves = 500;
    public Vector2 dist = new Vector2(0.1f, 1f);
    private bool startGen = false;
    private bool isGenerating = false;
    private bool isShown = false;
    private float curTimeoutTime = 0;
    public float maxTimeoutTime = 1;

    [Header("Mesh Generation")]
    [Range(0.5f, 4)]
    public float invertedGrowth = 1.5f;
    private Material meshMaterial;
    [Range(3, 10)]
    public int radialSubdivisions = 10;

    private Helper helper;

    private void Start() {
        helper = new Helper();
        meshMaterial = new Material(Shader.Find("Standard"));
    }

    void Update() {
        if (startGen) {
            if (isGenerating) {
                curTimeoutTime += Time.deltaTime;

                if (curTimeoutTime < maxTimeoutTime) {
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
        //reset timeout
        curTimeoutTime = 0;
        startGen = false;
        isGenerating = false;
        isShown = false;

        meshMaterial.color = branchColor;

        Debug.Log("Generating Tree...");

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
        tree.SCRef = this;

        tree.setup();

        startGen = true;
        isGenerating = true;

        Debug.Log("Finished Tree Generation");
    }

    public void ToggleLeaves() {
        tree.toggleLeaves();
    }

    public void ToggleShowLeafReferences() {
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}
