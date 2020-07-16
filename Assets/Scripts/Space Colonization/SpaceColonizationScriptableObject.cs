using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpaceColonizationData", menuName = "SpaceColonization/SpaceColonizationScriptableObject")]
public class SpaceColonizationScriptableObject : ScriptableObject {
    [Header("Leaf Colors")]
    public List<ProceduralColor> inputColors;
    public List<Color> leafColors = new List<Color>();
    public bool useReducedSubset = false;
    public int sublistLength = 8;

    [Header("Leaf Attributes")]
    public Vector2 leafSize = new Vector2(10, 20);
    [Range(0,50)]
    [Tooltip("Radius from node that leaves can spawn")]
    public float leafSpawnRadius = 1;
    [Range(1, 5)]
    public int numLeavesPerNode = 1;
    [Range(0,75)]
    [Tooltip("Distance between each leaf")]
    public float leafSpread = 1;

    [Header("Branch Attributes")]
    public Vector2 width = new Vector2(0.1f, 0.2f);
    public Vector2 length = new Vector2(0.1f, 0.2f);
    public Color branchColor = Color.white;

    [Header("Space Colonization Attributes")]
    public Vector3 rootPos = new Vector3(0, -200, 0);
    public int numLeafRef = 500;
    [Tooltip("Min and Max distance to check for node when spawning branches")]
    public Vector2 dist = new Vector2(0.1f, 1f);
    [Tooltip("Amount of time before algorithm stops spawning")]
    public float maxTimeoutTime = 1;

    [Header("Mesh Generation")]
    [Range(0.5f, 4)]
    public float invertedGrowth = 1.5f;
    [Range(3, 10)]
    public int radialSubdivisions = 10;

    [Header("Voxelization")]
    public int resolution = 32;

    private ColorHelper colorHelper;
    [HideInInspector]
    public bool showLeafRef;

    public void GenerateColors() {
        Debug.Log("Generating Colors...");
        colorHelper = new ColorHelper();
        leafColors = colorHelper.Generate(inputColors, useReducedSubset, sublistLength);
    }
}