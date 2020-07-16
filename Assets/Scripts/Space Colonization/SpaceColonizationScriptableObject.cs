using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpaceColonizationData", menuName = "SpaceColonization/SpaceColonizationScriptableObject")]
public class SpaceColonizationScriptableObject : ScriptableObject {
    [Header("Leaf Attributes")]
    public List<ProceduralColor> inputColors;
    public List<Color> leafColors = new List<Color>();
    public bool useReducedSubset = false;
    public int sublistLength = 8;
    public Vector2 leafSize = new Vector2(10, 20);
    [Range(0,50)]
    public float leafSpawnRadius = 1;
    [Range(1, 5)]
    public int numLeavesPerNode = 1;
    [Range(0,75)]
    public float leafSpread = 1;

    [Header("Branch Attributes")]
    public Vector2 width = new Vector2(0.1f, 0.2f);
    public Vector2 length = new Vector2(0.1f, 0.2f);
    public Color branchColor = Color.white;

    [Header("Space Colonization Attributes")]
    public Vector3 rootPos = new Vector3(0, -200, 0);
    public int numLeaves = 500;
    public Vector2 dist = new Vector2(0.1f, 1f);
    public float maxTimeoutTime = 1;

    [Header("Mesh Generation")]
    [Range(0.5f, 4)]
    public float invertedGrowth = 1.5f;
    
    [Range(3, 10)]
    public int radialSubdivisions = 10;

    private ColorHelper colorHelper;
    [HideInInspector]
    public bool showLeafRef;

    public void GenerateColors() {
        Debug.Log("Generating Colors...");
        colorHelper = new ColorHelper();
        leafColors = colorHelper.Generate(inputColors, useReducedSubset, sublistLength);
    }
}