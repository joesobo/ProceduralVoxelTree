using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch {
    public Vector3 position;
    public Vector3 direction;
    public Vector3 originalDir;
    public Branch parentBranch;
    public int leafCount = 0;
    public float length;
    public float width;

    public float size = 0;
    public List<Branch> children = new List<Branch>();

    public int verticesId;

    private Helper helper = new Helper();

    public Branch(Vector3 pos, Vector3 dir, Branch parentBranch, float length, float width) {
        this.position = pos;
        this.direction = dir;
        this.originalDir = this.direction;
        this.parentBranch = parentBranch;
        this.length = length;
        this.width = width;
    }

    public Branch next() {
        Vector3 nextDir = (direction * length);
        Vector3 nextPos = position + nextDir;
        Branch newBranch = new Branch(nextPos, direction, this, length, width);
        children.Add(newBranch);
        return newBranch;
    }

    public void reset() {
        direction = originalDir;
        leafCount = 0;
    }
}
