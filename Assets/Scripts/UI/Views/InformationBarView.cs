using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class InformationBarView : View
{
    public TextMesh fundsDisplay;
    public TextMesh researchDisplay;


    public void SetFunds(int funds)
    {
        fundsDisplay.text = "$" + funds;
    }

    public void SetResearch(float progress)
    {
        researchDisplay.text = Mathf.CeilToInt(progress * 100f) + "%";

        if (progress != 1f)
            researchDisplay.color = Color.black;
        else
            researchDisplay.color = Color.green;
    }
}
