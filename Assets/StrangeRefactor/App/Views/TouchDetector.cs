using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class TouchDetector : View
{
    public Signal OnUpAsButtonSignal = new Signal();
    public Signal OnDownSignal = new Signal();
    public Signal OnUpSignal = new Signal();

    protected override void Awake()
    {
        base.Awake();
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
    }

    private void OnMouseUpAsButton()
    {
        OnUpAsButtonSignal.Dispatch();
    }

    public void OnMouseDown()
    {
        OnDownSignal.Dispatch();
    }

    public void OnMouseUp()
    {
        OnUpSignal.Dispatch();
    }
}