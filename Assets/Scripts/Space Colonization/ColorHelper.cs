using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHelper {
    public List<Color> generate(List<ProceduralColor> inputColors, bool useReducedSubset, int sublistLength) {
        List<Color> leafColors = new List<Color>();
        List<Color> tempColorList;
        float hue, sat, val;

        //create variations
        foreach (ProceduralColor color in inputColors) {
            tempColorList = buildListOfColorVariations(color);

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
            leafColors = generateRandomSublist(leafColors, sublistLength);
        }

        return leafColors;
    }

    public List<Color> generateRandomSublist(List<Color> leafColors, int count) {
        List<Color> tempColorList = new List<Color>();
        for (int i = 0; i < count; i++) {
            tempColorList.Add(leafColors[Random.Range(0, leafColors.Count)]);
        }
        return tempColorList;
    }

    public List<Color> buildListOfColorVariations(ProceduralColor inputColor) {
        List<Color> tempColorList = new List<Color>();
        tempColorList.AddRange(generateSaturationVariations(inputColor));
        tempColorList.AddRange(generateValueVariations(inputColor));
        return tempColorList;
    }

    public List<Color> generateSaturationVariations(ProceduralColor inputColor) {
        List<Color> tempColorList = new List<Color>();
        float saturationIncrement = Random.Range(inputColor.saturation.x, inputColor.saturation.y) / inputColor.variationCount;

        for (int i = 1; i < inputColor.variationCount + 1; i++) {
            tempColorList.Add(desaturate(inputColor.color, saturationIncrement * i));
        }

        return tempColorList;
    }

    public List<Color> generateValueVariations(ProceduralColor inputColor) {
        List<Color> tempColorList = new List<Color>();
        float valueIncrement = Random.Range(inputColor.brightness.x, inputColor.brightness.y) / inputColor.variationCount;

        for (int i = 1; i < inputColor.variationCount + 1; i++) {
            tempColorList.Add(setValue(inputColor.color, valueIncrement * i));
        }

        return tempColorList;
    }

    public Color setValue(Color color, float value) {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);

        return Color.HSVToRGB(hue, sat, val * value);
    }

    public Color desaturate(Color color, float satuation) {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);

        return Color.HSVToRGB(hue, sat * satuation, val);
    }
}