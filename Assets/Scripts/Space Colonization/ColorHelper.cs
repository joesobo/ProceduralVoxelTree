using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHelper {
    public List<Color> Generate(List<ProceduralColor> inputColors, bool useReducedSubset, int sublistLength) {
        List<Color> leafColors = new List<Color>();
        List<Color> tempColorList;
        float hue, sat, val;

        //create variations
        foreach (ProceduralColor color in inputColors) {
            tempColorList = BuildListOfColorVariations(color);

            //filter
            for (int i = 0; i < tempColorList.Count; i++) {
                Color tempColor = tempColorList[i];
                Color.RGBToHSV(tempColor, out hue, out sat, out val);

                if (sat < color.minSaturation || val < color.minValue) {
                    tempColorList.RemoveAt(i);
                    i--;
                }
            }

            leafColors.AddRange(tempColorList);
        }

        //limit
        if (useReducedSubset) {
            leafColors = GenerateRandomSublist(leafColors, sublistLength);
        }

        return leafColors;
    }

    public List<Color> GenerateRandomSublist(List<Color> leafColors, int count) {
        List<Color> tempColorList = new List<Color>();
        for (int i = 0; i < count; i++) {
            tempColorList.Add(leafColors[Random.Range(0, leafColors.Count)]);
        }
        return tempColorList;
    }

    public List<Color> BuildListOfColorVariations(ProceduralColor inputColor) {
        List<Color> tempColorList = new List<Color>();
        tempColorList.AddRange(GenerateSaturationVariations(inputColor));
        tempColorList.AddRange(GenerateValueVariations(inputColor));
        return tempColorList;
    }

    public List<Color> GenerateSaturationVariations(ProceduralColor inputColor) {
        List<Color> tempColorList = new List<Color>();
        float saturationIncrement = Random.Range(inputColor.saturation.x, inputColor.saturation.y) / inputColor.variationCount;

        for (int i = 1; i < inputColor.variationCount + 1; i++) {
            tempColorList.Add(Desaturate(inputColor.color, saturationIncrement * i));
        }

        return tempColorList;
    }

    public List<Color> GenerateValueVariations(ProceduralColor inputColor) {
        List<Color> tempColorList = new List<Color>();
        float valueIncrement = Random.Range(inputColor.brightness.x, inputColor.brightness.y) / inputColor.variationCount;

        for (int i = 1; i < inputColor.variationCount + 1; i++) {
            tempColorList.Add(SetValue(inputColor.color, valueIncrement * i));
        }

        return tempColorList;
    }

    public Color SetValue(Color color, float value) {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);

        return Color.HSVToRGB(hue, sat, val * value);
    }

    public Color Desaturate(Color color, float satuation) {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);

        return Color.HSVToRGB(hue, sat * satuation, val);
    }
}