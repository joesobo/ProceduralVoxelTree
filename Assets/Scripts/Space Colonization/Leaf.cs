using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {
    public Vector3 position;
    public bool reached = false;
    private SpaceColonizationScriptableObject SCData;

    private GameObject objectRef;

    public Leaf(SpaceColonizationScriptableObject SCData) {
        this.SCData = SCData;

        //find point within mesh
        if (SCData.isConvexMesh) {
            this.position = (SelectRandomMeshPoints.GetRandomPointInsideConvex(SCData.leafRefShapeMesh) * SCData.leafRefSpawnSize) + SCData.leafRefOffset;
        } else {
            this.position = (SelectRandomMeshPoints.GetRandomPointInsideNonConvex(SCData.leafRefShapeMesh, SCData.leafRefOffset) * SCData.leafRefSpawnSize);
        }
    }

    public void show(Transform parent) {
        objectRef = GameObject.CreatePrimitive(PrimitiveType.Cube);
        objectRef.name = "Leaf Reference";
        objectRef.transform.position = position;
        objectRef.transform.parent = parent;
    }

    public void removeLeafRef() {
        Destroy(objectRef);
    }
}
