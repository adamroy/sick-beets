using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class BeetView : View
{
    public Signal OnClick = new Signal();

    private new Renderer renderer;
    private Color startColor;

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
        renderer.material.color = Color.red;
    }

    public void MarkUnselected()
    {
        renderer.material.color = startColor;
    }
}
