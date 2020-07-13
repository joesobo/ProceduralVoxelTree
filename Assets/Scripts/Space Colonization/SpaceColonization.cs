﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceColonization : MonoBehaviour {
    private Tree tree;
    private GameObject treeObject;

    public SpaceColonizationScriptableObject SCData;

    private bool startGen = false;
    private bool isGenerating = false;
    private bool isShown = false;
    private float curTimeoutTime = 0;

    private Material meshMaterial;

    private Helper helper;

    private void Start() {
        helper = new Helper();
        meshMaterial = new Material(Shader.Find("Standard"));
    }

    void Update() {
        if (startGen) {
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
        //reset timeout
        curTimeoutTime = 0;
        startGen = false;
        isGenerating = false;
        isShown = false;

        meshMaterial.color = SCData.branchColor;

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
        tree.SCData = SCData;
        tree.parent = this.transform;

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
