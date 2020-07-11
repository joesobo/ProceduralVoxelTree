using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    private MeshFilter filter;

    public SpaceColonization SCRef;

    private List<GameObject> leaves = new List<GameObject>();
    private List<Leaf> leavesRef = new List<Leaf>();
    private List<Branch> branches = new List<Branch>();

    public void setup() {
        filter = GetComponent<MeshFilter>();

        for (int i = 0; i < SCRef.numLeaves; i++) {
            leavesRef.Add(new Leaf());
        }

        Branch root = new Branch(SCRef.rootPos, Vector3.up, null, SCRef);

        branches.Add(root);

        Branch current = root;

        bool found = false;
        while (!found) {
            for (int i = 0; i < leavesRef.Count; i++) {
                float dist = Vector3.Distance(current.position, leavesRef[i].position);

                if (dist < SCRef.dist.y) {
                    found = true;
                }
            }

            if (!found) {
                Branch branch = current.next();
                current = branch;
                branches.Add(current);
            }
        }
    }

    public void grow() {
        for (int i = 0; i < leavesRef.Count; i++) {
            Leaf leaf = leavesRef[i];

            Branch closestBranch = null;
            float record = 100000000;
            for (int j = 0; j < branches.Count; j++) {
                Branch branch = branches[j];
                float dist = Vector3.Distance(leaf.position, branch.position);

                if (dist < SCRef.dist.x) {
                    leaf.reached = true;
                    closestBranch = null;
                    break;
                } else if (dist > SCRef.dist.y) {

                } else if (closestBranch == null || dist < record) {
                    closestBranch = branch;
                    record = dist;
                }
            }

            if (closestBranch != null) {
                Vector3 newDir = leaf.position - closestBranch.position;
                newDir.Normalize();
                closestBranch.direction += newDir;
                closestBranch.leafCount++;
            }
        }

        //create leaves and remove reference upon reaching
        for (int i = leavesRef.Count - 1; i >= 0; i--) {
            if (leavesRef[i].reached) {
                //Create new leaf
                GameObject newLeaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newLeaf.name = "Leaf";
                newLeaf.GetComponent<MeshRenderer>().material = SCRef.leafMat;
                newLeaf.transform.position = leavesRef[i].position;
                newLeaf.transform.localScale = Vector3.one * Random.Range(10, 20);
                newLeaf.transform.parent = this.transform;
                leaves.Add(newLeaf);

                //Remove reference
                leavesRef[i].RemoveLeafRef();
                leavesRef.RemoveAt(i);
            }
        }

        //actually grow branch
        for (int i = branches.Count - 1; i >= 0; i--) {
            Branch branch = branches[i];

            if (branch.leafCount > 0) {
                branch.direction /= branch.leafCount + 1;

                branches.Add(branch.next());
            }

            branch.reset();
        }
    }

    public void show() {
        Mesh treeMesh = new Mesh();

        for (int i = 0; i < leavesRef.Count; i++) {
            leavesRef[i].show(SCRef.transform);
        }

        for (int i = branches.Count - 1; i >= 0; i--) {
            float newSize = 0;
            Branch b = branches[i];

            if (b.children.Count == 0) {
                newSize = Random.Range(b.SCRef.width.x, b.SCRef.width.y);
            } else {
                foreach (Branch bc in b.children) {
                    newSize += Mathf.Pow(bc.size, SCRef.invertedGrowth);
                }
                newSize = Mathf.Pow(newSize, 1f / SCRef.invertedGrowth);
            }
            b.size = newSize;
        }

        Vector3[] vertices = new Vector3[(branches.Count + 1) * SCRef.radialSubdivisions];
        int[] triangles = new int[branches.Count * SCRef.radialSubdivisions * 6];

        for (int i = 0; i < branches.Count; i++) {
            Branch b = branches[i];

            int vertIndex = SCRef.radialSubdivisions * i;
            b.verticesId = vertIndex;

            Quaternion quat = Quaternion.FromToRotation(Vector3.up, b.direction);

            for (int s = 0; s < SCRef.radialSubdivisions; s++) {
                float alpha = ((float)s / SCRef.radialSubdivisions) * Mathf.PI * 2f;

                Vector3 pos = new Vector3(Mathf.Cos(alpha) * b.size, 0, Mathf.Sin(alpha) * b.size);
                pos = quat * pos;
                pos += b.position + (b.direction * Random.Range(b.SCRef.length.x, b.SCRef.length.y));
                vertices[vertIndex + s] = pos - SCRef.rootPos;

                if (b.parentBranch == null) {
                    vertices[branches.Count * SCRef.radialSubdivisions + s] = b.position + new Vector3(Mathf.Cos(alpha) * b.size, 0, Mathf.Sin(alpha) * b.size) - SCRef.rootPos;
                }
            }
        }

        for (int i = 0; i < branches.Count; i++) {
            Branch b = branches[i];
            int fid = i * SCRef.radialSubdivisions * 2 * 3;
            int bId = b.parentBranch != null ? b.parentBranch.verticesId : branches.Count * SCRef.radialSubdivisions;
            int tId = b.verticesId;

            for (int s = 0; s < SCRef.radialSubdivisions; s++) {
                triangles[fid + s * 6] = bId + s;
                triangles[fid + s * 6 + 1] = tId + s;

                if (s == SCRef.radialSubdivisions - 1) {
                    triangles[fid + s * 6 + 2] = tId;
                } else {
                    triangles[fid + s * 6 + 2] = tId + s + 1;
                }

                if (s == SCRef.radialSubdivisions - 1) {
                    triangles[fid + s * 6 + 3] = bId + s;
                    triangles[fid + s * 6 + 4] = tId;
                    triangles[fid + s * 6 + 5] = bId;
                } else {
                    triangles[fid + s * 6 + 3] = bId + s;
                    triangles[fid + s * 6 + 4] = tId + s + 1;
                    triangles[fid + s * 6 + 5] = bId + s + 1;
                }
            }
        }

        treeMesh.vertices = vertices;
        treeMesh.triangles = triangles;
        treeMesh.RecalculateNormals();
        filter.mesh = treeMesh;
    }

    public void toggleLeaves() {
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}