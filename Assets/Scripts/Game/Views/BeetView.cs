using UnityEngine;
using System.Collections.Generic;
using System;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using Random = UnityEngine.Random;

public class BeetView : View
{
    public Signal OnClick = new Signal();

    [Serializable]
    public class EnvironmentNeed
    {
        public EnvironmentVariable variable;
        [Range(0f, 1f)]
        public float min, max;

        private float value = -1;
        public float Value
        {
            get
            {
                if (value < 0f)
                    value = Random.Range(min, max);
                return value;
            }
        }
    }

    public List<EnvironmentNeed> environmentNeeds;

    private new Renderer renderer;
    private Color startColor;
    private bool isSelected;

    private void OnMouseUpAsButton()
    {
        OnClick.Dispatch();
    }

    public void Init()
    {
        renderer = GetComponentInChildren<Renderer>();
        startColor = renderer.material.color;
    }

    public void MarkSelected()
    {
        isSelected = true;
        if (renderer != null)
            renderer.material.color = Color.red;
    }

    public void MarkUnselected()
    {
        isSelected = false;
        if (renderer != null)
            renderer.material.color = startColor;
    }

    public void SetHealthIndicator(float health)
    {
        if (!isSelected)
        {
            renderer.material.color = Color.Lerp(startColor, Color.green, health);
        }
    }
}
