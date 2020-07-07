using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    private MeshFilter filter;
    public GameObject leafPrefab;
    public Transform parent;
    private List<Leaf> leaves = new List<Leaf>();
    private List<Branch> branches = new List<Branch>();
    public Transform treeParent;
    public float invertedGrowth;

    public float width;
    public float length;

    public float maxDist = 1f;
    public float minDist = .1f;
    public int numLeaves = 100;
    public int radialSubdivisions = 10;
    private Vector3 rootPos = new Vector3(0, -200, 0);

    public void setup() {
        filter = GetComponent<MeshFilter>();
        
        for (int i = 0; i < numLeaves; i++) {
            leaves.Add(new Leaf());
        }

        Branch root = new Branch(rootPos, Vector3.up, null, length, width);

        branches.Add(root);

        Branch current = root;

        bool found = false;
        while (!found) {
            for (int i = 0; i < leaves.Count; i++) {
                float dist = Vector3.Distance(current.position, leaves[i].position);

                if (dist < maxDist) {
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
        for (int i = 0; i < leaves.Count; i++) {
            Leaf leaf = leaves[i];

            Branch closestBranch = null;
            float record = 100000000;
            for (int j = 0; j < branches.Count; j++) {
                Branch branch = branches[j];
                float dist = Vector3.Distance(leaf.position, branch.position);

                if (dist < minDist) {
                    leaf.reached = true;
                    closestBranch = null;
                    break;
                } else if (dist > maxDist) {

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

        //trim reached leaves (delete reference)
        for (int i = leaves.Count - 1; i >= 0; i--) {
            if (leaves[i].reached) {
                leaves[i].RemoveLeaf();
                leaves.RemoveAt(i);
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

        for (int i = 0; i < leaves.Count; i++) {
            leaves[i].show(leafPrefab, parent);
        }

        for (int i = branches.Count - 1; i >= 0; i--) {
            float newSize = 0;
            Branch b = branches[i];

            if (b.children.Count == 0) {
                newSize = b.width;
            } else {
                foreach (Branch bc in b.children) {
                    newSize += Mathf.Pow(bc.size, invertedGrowth);
                }
                newSize = Mathf.Pow(newSize, 1f / invertedGrowth);
            }
            b.size = newSize;
        }

        Vector3[] vertices = new Vector3[(branches.Count + 1) * radialSubdivisions];
        int[] triangles = new int[branches.Count * radialSubdivisions * 6];

        for (int i = 0; i < branches.Count; i++) {
            Branch b = branches[i];

            int vertIndex = radialSubdivisions * i;
            b.verticesId = vertIndex;

            Quaternion quat = Quaternion.FromToRotation(Vector3.up, b.direction);

            for (int s = 0; s < radialSubdivisions; s++) {
                float alpha = ((float)s / radialSubdivisions) * Mathf.PI * 2f;

                Vector3 pos = new Vector3(Mathf.Cos(alpha) * b.size, 0, Mathf.Sin(alpha) * b.size);
                pos = quat * pos;
                pos += b.position + (b.direction * b.length);
                vertices[vertIndex + s] = pos - rootPos;

                if (b.parentBranch == null) {
                    vertices[branches.Count * radialSubdivisions + s] = b.position + new Vector3(Mathf.Cos(alpha) * b.size, 0, Mathf.Sin(alpha) * b.size) - rootPos;
                }
            }
        }

        for (int i = 0; i < branches.Count; i++) {
            Branch b = branches[i];
            int fid = i * radialSubdivisions * 2 * 3;
            int bId = b.parentBranch != null ? b.parentBranch.verticesId : branches.Count * radialSubdivisions;
            int tId = b.verticesId;

            for (int s = 0; s < radialSubdivisions; s++) {
                triangles[fid+s*6] = bId + s;
                triangles[fid+s*6+1] = tId + s;

                if (s == radialSubdivisions-1) {
                    triangles[fid+s*6+2] = tId;
                } else {
                    triangles[fid+s*6+2] = tId + s + 1;
                }

                if (s == radialSubdivisions-1) {
					triangles[fid+s*6+3] = bId + s;
					triangles[fid+s*6+4] = tId;
					triangles[fid+s*6+5] = bId;
				} else {
					triangles[fid+s*6+3] = bId + s;
					triangles[fid+s*6+4] = tId + s + 1;
					triangles[fid+s*6+5] = bId + s + 1;
				}
            }
        }

        treeMesh.vertices = vertices;
        treeMesh.triangles = triangles;
        treeMesh.RecalculateNormals();
        filter.mesh = treeMesh;
    }
}