using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public Vector3 center = Vector3.zero;
    private Vector3 dir = Vector3.up;

    private void Update() {
        transform.RotateAround(center, dir, rotateSpeed * Time.deltaTime);
    }
}
