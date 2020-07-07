using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rule {
    public string a;
    public string b;

    public Rule(string a, string b) {
        this.a = a;
        this.b = b;
    }
}

public class LSystem : MonoBehaviour {
    public Helper helper = new Helper();
    public Text textDisplay;
    public Material lineMaterial;

    public float lineLength = 1;
    public float lineWidth = 0.1f;
    public int angle = 30;

    private string axiom = "F";
    private string sentence;
    private List<Rule> rules = new List<Rule>();

    void Start() {
        sentence = axiom;
        textDisplay.text = "Sentence: " + sentence;

        rules.Add(new Rule("F", "FF+[+F-F-F]-[-F+F+F]"));

        turtle();
    }

    public void generate() {
        print("Generating...");
        lineLength *= 0.5f;
        lineWidth *= 0.75f;
        string nextSentence = "";
        for (int i = 0; i < sentence.Length; i++) {
            string current = "" + sentence[i];
            bool found = false;
            for (int j = 0; j < rules.Count; j++) {
                if (current == rules[j].a) {
                    nextSentence += rules[j].b;
                    found = true;
                    break;
                }
            }

            if (!found) {
                nextSentence += current;
            }
        }

        sentence = nextSentence;
        textDisplay.text = "Sentence: " + sentence;

        helper.ClearAllChildren(this.transform);

        turtle();
    }

    public void turtle() {
        Stack<Vector3> points = new Stack<Vector3>();
        Stack<Vector3> headings = new Stack<Vector3>();

        Vector3 startPoint = Vector3.zero;
        Vector3 heading = Vector3.up;

        for (int i = 0; i < sentence.Length; i++) {
            string current = "" + sentence[i];

            if (current == "F") {
                helper.DrawLine(startPoint, startPoint + (heading * lineLength), this.transform, lineMaterial, lineWidth);
                startPoint += heading * lineLength;
            } else if (current == "+") {
                heading = Quaternion.Euler(0, 0, angle) * heading;
            } else if (current == "-") {
                heading = Quaternion.Euler(0, 0, -angle) * heading;
            } else if (current == "[") {
                points.Push(startPoint);
                headings.Push(heading);
            } else if (current == "]") {
                startPoint = points.Pop();
                heading = headings.Pop();
            }
        }
    }
}
