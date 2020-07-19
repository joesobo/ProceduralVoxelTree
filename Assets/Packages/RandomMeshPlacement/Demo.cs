using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// To demonstrate the random generation of points within a mesh.
/// 
/// If you assume non-convexity, you should place the child transform named "Center" 
/// somewhere in your mesh from where you can see most of the vertices 
/// </summary>
public class Demo : MonoBehaviour {
    
    /// <summary>
    /// Assume Convexity? If not, place the Center as directed.
    /// </summary>
    public bool isConvex = false;
    /// <summary>
    /// Place somewhere inside your mesh, from where you can see most of the vertices.
    /// </summary>
    public Transform center;

    /// <summary>
    /// Rate of spawning - press "T" to instantly spawn more spheres.
    /// </summary>
    public int pointsToAddPerSecond = 50;

    MeshFilter mF;
    List<Point> points; 
    void Start()
    {
        mF = GetComponent<MeshFilter>();
        points = new List<Point>(); 
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
            Populate(5 * pointsToAddPerSecond, 10); 
    }
    void FixedUpdate()
    {
        Populate(Mathf.CeilToInt(pointsToAddPerSecond * Time.fixedDeltaTime), 10);
    }

    void Populate(int numRays, float duration = 0)
    {
        if (duration <= 0)
            duration = Time.deltaTime;
        if (numRays <= 0)
            numRays = 1;

        for (int i = 0; i < numRays; i ++)
        {
            // Vector3 point = mF.mesh.GetRandomPointOnSurface();
            Vector3 point = isConvex ? mF.mesh.GetRandomPointInsideConvex() : mF.mesh.GetRandomPointInsideNonConvex(center.localPosition);
            points.Add(new Point(point));
        } 
    }
    IEnumerator DelayedDelete(Point p)
    {
        yield return new WaitForSeconds(10);
        points.Remove(p);
    }

    void OnDrawGizmos()
    {
        if (points == null || points.Count == 0)
            return;

        foreach(Point p in points)
        {
            Gizmos.color = Color.red; // The_Helper.InterpolateColor(Color.red, Color.green, p.pos.magnitude); 

            Gizmos.DrawSphere(transform.TransformPoint(p.pos), transform.lossyScale.magnitude / 100);
        }
    }
    struct Point
    { 
        public Point(Vector3 pos)
        {
            this.pos = pos;
        }
        public Vector3 pos;
    }
}
