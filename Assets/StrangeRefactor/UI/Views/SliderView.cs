using UnityEngine;
using System;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class SliderView : View
{
    public Signal<SliderView> OnValueChanged = new Signal<SliderView>();
    public float Value
    {
        get
        {
            return value;
        }
        private set
        {
            this.value = value;
            sliderGameObject.transform.position = Vector3.Lerp(lowEndTransform.position, highEndTransform.position, value);
            OnValueChanged.Dispatch(this);
        }
    }

    public Transform lowEndTransform, highEndTransform;
    public GameObject sliderGameObject;
    public new Camera camera;

    private float value = 0.5f;
    private bool sliderHeld;
    
    public void Init()
    {
        sliderHeld = false;
        var touchDetector = sliderGameObject.AddComponent<TouchDetector>();
        touchDetector.OnDownSignal.AddListener(() => sliderHeld = true);
        touchDetector.OnUpSignal.AddListener(() => sliderHeld = false);
        Value = Value;
    }

    private void Update()
    {
        if(sliderHeld)
        {
            var p = new Plane(-camera.transform.forward, lowEndTransform.position);
            var mouseRay = camera.ScreenPointToRay(Input.mousePosition);
            float d;
            if(p.Raycast(mouseRay, out d))
            {
                var mousePoint = mouseRay.GetPoint(d);
                Value = ProjectPointToLineAndFindProgress(lowEndTransform.position, highEndTransform.position, mousePoint);
            }
        }
    }

    private float ProjectPointToLineAndFindProgress(Vector3 a, Vector3 b, Vector3 p)
    {
        var ab = a - b;
        var ap = a - p;
        var result = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
        float t = Vector3.Dot(ab, result) / (ab.magnitude * ab.magnitude);
        t = Mathf.Clamp01(t);
        return t;
    }
}
