using UnityEngine;
using System.Collections;

public static class SelectRandomMeshPoints 
{ 
    /// <summary>
    /// Picks a random point inside a CONVEX mesh.
    /// Taking advantage of Convexity, we can produce more evenly distributed points
    /// </summary> 
    public static Vector3 GetRandomPointInsideConvex(this Mesh m)
    {
        // Grab two points on the surface
        Vector3 randomPointOnSurfaceA = m.GetRandomPointOnSurface();
        Vector3 randomPointOnSurfaceB = m.GetRandomPointOnSurface();

        // Interpolate between them
        return Vector3.Lerp(randomPointOnSurfaceA, randomPointOnSurfaceB, Random.Range(0f, 1f));
    }

    /// <summary>
    /// Picks a random point inside a NON-CONVEX mesh.
    /// The only way to get good approximations is by providing a point (if there is one)
    /// that has line of sight to most other points in the non-convex shape.
    /// </summary> 
    public static Vector3 GetRandomPointInsideNonConvex(this Mesh m, Vector3 pointWhichSeesAll)
    {
        // Grab one point (and the center which we assume has line of sight with this point)
        Vector3 randomPointOnSurface = m.GetRandomPointOnSurface(); 

        // Interpolate between them
        return Vector3.Lerp(pointWhichSeesAll, randomPointOnSurface, Random.Range(0f, 1f));
    }


    /// <summary>
    /// Picks a random point on the mesh's surface.
    /// </summary> 
    public static Vector3 GetRandomPointOnSurface(this Mesh m, Transform debugTransform = null)
    {
        // Pick a random triangle (each triangle is 3 integers in a row in m.triangles)
        // So Pick a random origin (0, 3, 6, .. m.triangles.Length - 3)
        // -> Random (0.. m.triangles.Length / 3) * 3
        int triangleOrigin = Mathf.FloorToInt(UnityEngine.Random.Range(0f, m.triangles.Length) / 3f) * 3;

        // Grab the 3 points that consist of the triangle
        Vector3 vertexA = m.vertices[m.triangles[triangleOrigin]];
        Vector3 vertexB = m.vertices[m.triangles[triangleOrigin + 1]];
        Vector3 vertexC = m.vertices[m.triangles[triangleOrigin + 2]];

        // Pick a random point on the triangle
        // For a uniform distribution, we pick randomly according to this:
        // http://mathworld.wolfram.com/TrianglePointPicking.html
        // From the point of origin (vertexA) move a random distance towards vertexB and from there a random distance in the direction of (vertexC - vertexB)
        // The only (temporary) downside is that we might end up with points outside our triangle as well, which have to be mapped back
        // The good thing is that these points can only end up in the triangle's "reflection" across the AC side (forming a quad AB, BC, CD, DA)

        Vector3 dAB = vertexB - vertexA;
        Vector3 dBC = vertexC - vertexB;

        float rAB = UnityEngine.Random.Range(0f, 1f);
        float rBC = UnityEngine.Random.Range(0f, 1f);

        Vector3 randPoint = vertexA + rAB * dAB + rBC * dBC;

        // We have produces random points on a quad (the extension of our triangle)
        // To map back to the triangle, first we check if we are on the extension of the triangle
        // Since we can be on one of two triangles this is equivalent with checking if we are on the correct side of the AC line
        // If we are on the correct side (towards B) we are on the triangle - else we are not.

        // To check that we can compare the direction of our point towards any point on that line (say, C)
        // with the direction of the height of side AC (Cross (triangleNormal, dirBC)))
        Vector3 dirPC = (vertexC - randPoint).normalized;

        Vector3 dirAB = (vertexB - vertexA).normalized;
        Vector3 dirAC = (vertexC - vertexA).normalized;

        Vector3 triangleNormal = Vector3.Cross(dirAC, dirAB).normalized;

        Vector3 dirH_AC = Vector3.Cross(triangleNormal, dirAC).normalized;

        // If the two are alligned, we're in the wrong side
        float dot = Vector3.Dot(dirPC, dirH_AC);

        // We are on the right side, we're done
        if (dot >= 0)
        {
            // Otherwise, we need to find the symmetric to the center of the "quad" which is on the intersection of side AC with the bisecting line of angle (BA, BC)
            // Given by
            Vector3 centralPoint = (vertexA + vertexC) / 2;

            // And the symmetric point is given by the equation c - p = p_Sym - c => p_Sym = 2c - p
            Vector3 symmetricRandPoint = 2 * centralPoint - randPoint;

            if (debugTransform)
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(symmetricRandPoint), Color.red, 10);
            randPoint = symmetricRandPoint;
        }

        // For debugging purposes
        if (debugTransform)
        {
            Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexA), Color.cyan, 10);
            Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexB), Color.green, 10);
            Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexC), Color.blue, 10);
            // Debug.DrawRay(debugTransform.TransformPoint(randPoint), triangleNormal, Color.cyan, 10); 
        }

        return randPoint;
    }

    /// <summary>
    /// Returns the mesh's center.
    /// </summary> 
    public static Vector3 GetCenterPoint(this Mesh m)
    {
        Vector3 center = Vector3.zero;
        foreach (Vector3 v in m.vertices)
            center += v;
        return center / m.vertexCount;
    }
}
