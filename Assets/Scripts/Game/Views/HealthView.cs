using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class HealthView : View
{
    public Color[] healthColors;

    public SpriteRenderer healthBar;

    public void DisplayHealth(float health)
    {
        // Set the color
        int firstIndex = Mathf.FloorToInt((health - 0.001f) * (healthColors.Length - 1));
        float progress = health * (healthColors.Length - 1) - firstIndex;
        int secondIndex = firstIndex + 1;
        healthBar.color = Color.Lerp(healthColors[firstIndex], healthColors[secondIndex], progress);
        
        // Set the bar width
        var scale = healthBar.transform.localScale;
        scale.x = health;
        healthBar.transform.localScale = scale;
    }
}
