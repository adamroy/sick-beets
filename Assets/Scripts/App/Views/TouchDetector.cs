using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class TouchDetector : View
{
    public Signal OnUpAsButtonSignal = new Signal();
    public Signal OnDownSignal = new Signal();
    public Signal OnUpSignal = new Signal();
    public Camera RaycastCamera { get; set; }

    private bool mouseDownOverCollider;

    protected override void Awake()
    {
        if (RaycastCamera == null) RaycastCamera = Camera.main;
        base.Awake();
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
        GetComponent<Collider>().isTrigger = false;
    }

    private void MouseUpAsButton()
    {
        OnUpAsButtonSignal.Dispatch();
    }

    private void MouseDown()
    {
        OnDownSignal.Dispatch();
    }

    private void MouseUp()
    {
        OnUpSignal.Dispatch();
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(RaycastHit())
            { 
                mouseDownOverCollider = true;
                MouseDown();
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (RaycastHit())
            {
                if (mouseDownOverCollider)
                {
                    MouseUpAsButton();
                }

                MouseUp();
            }
            else
            {
                if (mouseDownOverCollider)
                    MouseUp();
            }

            mouseDownOverCollider = false;
        }
    }

    private bool RaycastHit()
    {
        RaycastHit hit;
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100f) && hit.collider == GetComponent<Collider>();
    }
}