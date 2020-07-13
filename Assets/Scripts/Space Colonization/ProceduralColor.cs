using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralColor {
    public Color color = Color.white;
    public Vector2 saturation = new Vector2(0, 1);
    public Vector2 brightness = new Vector2(0, 1);

    public float minSaturation = 0.25f;
    public float minValue = 0.25f;
    public int variationCount = 8;
}