using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class HealingRateView : View
{
    public SpriteRenderer healingRateDisplay;
    public Sprite[] healingRateIcons;

    protected override void Start()
    {
        base.Start();
        if (healingRateIcons.Length != 3) Debug.LogError("Expected 3 healing rate icons.", this);
        DisplayHealingRate(0.5f);
    }

    public void DisplayHealingRate(float rate)
    {
        if (rate < -0.2f) healingRateDisplay.sprite = healingRateIcons[0];
        else if (rate >= -0.2f && rate <= 0.2f) healingRateDisplay.sprite = healingRateIcons[1];
        else healingRateDisplay.sprite = healingRateIcons[2];
    }
}
