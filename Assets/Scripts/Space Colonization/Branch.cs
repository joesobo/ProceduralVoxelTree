using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch {
    public Vector3 position;
    public Vector3 direction;
    public Vector3 originalDir;
    public Branch parentBranch;
    public int leafCount = 0;
    public Vector2 width;
    public Vector2 length;

    public float size = 0;
    public List<Branch> children = new List<Branch>();

    public int verticesId;

    private Helper helper = new Helper();

    public Branch(Vector3 pos, Vector3 dir, Branch parentBranch, Vector2 width, Vector2 length) {
        this.position = pos;
        this.direction = dir;
        this.originalDir = this.direction;
        this.parentBranch = parentBranch;
        this.width = width;
        this.length = length;
    }

    public Branch next() {
        Vector3 nextDir = (direction * Random.Range(length.x, length.y));
        Vector3 nextPos = position + nextDir;
        Branch newBranch = new Branch(nextPos, direction, this, width, length);
        children.Add(newBranch);
        return newBranch;
    }

    public void reset() {
        direction = originalDir;
        leafCount = 0;
    }
}
