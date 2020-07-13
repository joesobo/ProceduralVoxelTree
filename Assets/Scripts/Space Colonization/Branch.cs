using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch {
    public SpaceColonizationScriptableObject scData;

    public Vector3 position;
    public Vector3 direction;
    public Vector3 originalDir;
    public Branch parentBranch;
    public int leafCount = 0;

    public float size = 0;
    public List<Branch> children = new List<Branch>();

    public int verticesId;

    private Helper helper = new Helper();

    public Branch(Vector3 pos, Vector3 dir, Branch parentBranch, SpaceColonizationScriptableObject scData) {
        this.position = pos;
        this.direction = dir;
        this.originalDir = this.direction;
        this.parentBranch = parentBranch;
        this.scData = scData;
    }

    public Branch next() {
        Vector3 nextDir = (direction * Random.Range(scData.length.x, scData.length.y));
        Vector3 nextPos = position + nextDir;
        Branch newBranch = new Branch(nextPos, direction, this, scData);
        children.Add(newBranch);
        return newBranch;
    }

    public void reset() {
        direction = originalDir;
        leafCount = 0;
    }
}
