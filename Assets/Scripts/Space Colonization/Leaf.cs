using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {
    public Vector3 position;
    public bool reached = false;

    private GameObject objectRef;

    public Leaf() {
        this.position = Random.insideUnitSphere * 200 + new Vector3(0, 50, 0);
        //this.position = new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));
    }

    public void show(GameObject prefab, Transform parent) {
        objectRef = Instantiate(prefab, position, Quaternion.identity, parent);
    }

    public void RemoveLeaf() {
        Destroy(objectRef);
    }
}
