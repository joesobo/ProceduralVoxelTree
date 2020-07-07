using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper {
    public void DrawLine(Vector3 start, Vector3 end, Transform parent, Material lineMat, float lineWidth) {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.transform.parent = parent;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = lineMat;
        lr.startColor = lineMat.GetColor("_TintColor");
        lr.endColor = lineMat.GetColor("_TintColor");

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, lineWidth);
        curve.AddKey(1, lineWidth);

        lr.widthCurve = curve;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void ClearAllChildren(Transform transform) {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}