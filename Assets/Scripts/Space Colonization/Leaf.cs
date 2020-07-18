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

        if (SCData.leafRefShape == SpaceColonizationScriptableObject.LeafRefShape.Sphere) {
            this.position = Random.insideUnitSphere * SCData.leafRefSpawnSize + SCData.leafRefOffset;
        } else{
            this.position = new Vector3(Random.Range(-SCData.leafRefSpawnSize, SCData.leafRefSpawnSize), 
                                        Random.Range(-SCData.leafRefSpawnSize, SCData.leafRefSpawnSize),
                                        Random.Range(-SCData.leafRefSpawnSize, SCData.leafRefSpawnSize));
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
