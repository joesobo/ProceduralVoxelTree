using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    private MeshFilter filter;

    public SpaceColonizationScriptableObject SCData;
    public SpaceColonization parent;

    private List<GameObject> leaves = new List<GameObject>();
    private List<Leaf> leavesRef = new List<Leaf>();
    private List<Branch> branches = new List<Branch>();

    public void setup() {
        filter = GetComponent<MeshFilter>();

        for (int i = 0; i < SCData.numLeaves; i++) {
            leavesRef.Add(new Leaf());
        }

        Branch root = new Branch(SCData.rootPos, Vector3.up, null, SCData);

        branches.Add(root);

        Branch current = root;

        bool found = false;
        while (!found) {
            for (int i = 0; i < leavesRef.Count; i++) {
                float dist = Vector3.Distance(current.position, leavesRef[i].position);

                if (dist < SCData.dist.y) {
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

                if (dist < SCData.dist.x) {
                    leaf.reached = true;
                    closestBranch = null;
                    break;
                } else if (dist > SCData.dist.y) {

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
                CreateLeaves(i);

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

        if (SCData.showLeafRef) {
            for (int i = 0; i < leavesRef.Count; i++) {
                leavesRef[i].show(parent.transform);
            }
        }

        for (int i = branches.Count - 1; i >= 0; i--) {
            float newSize = 0;
            Branch b = branches[i];

            if (b.children.Count == 0) {
                newSize = Random.Range(b.scData.width.x, b.scData.width.y);
            } else {
                foreach (Branch bc in b.children) {
                    newSize += Mathf.Pow(bc.size, SCData.invertedGrowth);
                }
                newSize = Mathf.Pow(newSize, 1f / SCData.invertedGrowth);
            }
            b.size = newSize;
        }

        Vector3[] vertices = new Vector3[(branches.Count + 1) * SCData.radialSubdivisions];
        int[] triangles = new int[branches.Count * SCData.radialSubdivisions * 6];

        for (int i = 0; i < branches.Count; i++) {
            Branch b = branches[i];

            int vertIndex = SCData.radialSubdivisions * i;
            b.verticesId = vertIndex;

            Quaternion quat = Quaternion.FromToRotation(Vector3.up, b.direction);

            for (int s = 0; s < SCData.radialSubdivisions; s++) {
                float alpha = ((float)s / SCData.radialSubdivisions) * Mathf.PI * 2f;

                Vector3 pos = new Vector3(Mathf.Cos(alpha) * b.size, 0, Mathf.Sin(alpha) * b.size);
                pos = quat * pos;
                pos += b.position + (b.direction * Random.Range(b.scData.length.x, b.scData.length.y));
                vertices[vertIndex + s] = pos - SCData.rootPos;

                if (b.parentBranch == null) {
                    vertices[branches.Count * SCData.radialSubdivisions + s] = b.position + new Vector3(Mathf.Cos(alpha) * b.size, 0, Mathf.Sin(alpha) * b.size) - SCData.rootPos;
                }
            }
        }

        for (int i = 0; i < branches.Count; i++) {
            Branch b = branches[i];
            int fid = i * SCData.radialSubdivisions * 2 * 3;
            int bId = b.parentBranch != null ? b.parentBranch.verticesId : branches.Count * SCData.radialSubdivisions;
            int tId = b.verticesId;

            for (int s = 0; s < SCData.radialSubdivisions; s++) {
                triangles[fid + s * 6] = bId + s;
                triangles[fid + s * 6 + 1] = tId + s;

                if (s == SCData.radialSubdivisions - 1) {
                    triangles[fid + s * 6 + 2] = tId;
                } else {
                    triangles[fid + s * 6 + 2] = tId + s + 1;
                }

                if (s == SCData.radialSubdivisions - 1) {
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

    public void CreateLeaves(int index) {
        Vector3 leafPos;
        Collider[] hitColliders;

        for (int i = 0; i < SCData.numLeavesPerNode; i++) {
            leafPos = (Random.insideUnitSphere * SCData.leafSpawnRadius) + leavesRef[index].position;
            hitColliders = Physics.OverlapSphere(leafPos, SCData.leafSpread);

            if (hitColliders.Length == 0) {
                CreateLeaf(leafPos);
                Physics.SyncTransforms();
            }
        }
    }

    public void CreateLeaf(Vector3 pos) {
        GameObject newLeaf;
        Material leafMat;

        newLeaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newLeaf.name = "Leaf";

        leafMat = new Material(Shader.Find("Standard"));

        leafMat.color = RandomColorHueBased();

        newLeaf.GetComponent<MeshRenderer>().material = leafMat;

        newLeaf.transform.position = pos;
        newLeaf.transform.localScale = Vector3.one * Random.Range(SCData.leafSize.x, SCData.leafSize.y);
        newLeaf.transform.parent = this.parent.treeLeaves.transform;
        leaves.Add(newLeaf);
    }

    public Color RandomColorHueBased() {
        return SCData.leafColors[Random.Range(0, SCData.leafColors.Count - 1)];
    }
}