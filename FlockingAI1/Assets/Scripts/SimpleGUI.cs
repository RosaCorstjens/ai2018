using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGUI : MonoBehaviour {

    public float sight = 100f;
    public float space = 50f;

    public float scaleS = 3.5f;
    public float scaleC = 0.01f;
    public float scaleA = 1f;

    private void OnGUI() {

        GUI.Label(new Rect(20, 15, 100, 20), "sight:");
        sight = GUI.HorizontalSlider(new Rect(120, 20, 150, 20), sight, 0, 150f);

        GUI.Label(new Rect(20, 35, 100, 20), "space:");
        space = GUI.HorizontalSlider(new Rect(120, 40, 150, 20), space, 0, 150f);


        GUI.Label(new Rect(20, 75, 100, 20), "Seperation:");
        scaleS = GUI.HorizontalSlider(new Rect(120, 80, 150, 20), scaleS, 0, 10f);

        GUI.Label(new Rect(20, 95, 100, 20), "Cohesion:");
        scaleC = GUI.HorizontalSlider(new Rect(120, 100, 150, 20), scaleC, 0, 10f);

        GUI.Label(new Rect(20, 115, 100, 20), "Alignment:");
        scaleA = GUI.HorizontalSlider(new Rect(120, 120, 150, 20), scaleA, 0, 10f);


        Agent.sight = sight;
        Agent.space = space;
        Agent.scaleFactorS = scaleS;
        Agent.scaleFactorC = scaleC;
        Agent.scaleFactorA = scaleA;
    }
}
