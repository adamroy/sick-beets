using UnityEngine;
using System;
using System.Collections;

public class Slider : MonoBehaviour
{
    public Action<Slider> OnValueChanged;
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
            if (OnValueChanged != null) OnValueChanged(this);
        }
    }

    public Transform lowEndTransform, highEndTransform;
    public TouchSensor sliderSensor;
    public GameObject sliderGameObject;
    public new Camera camera;
    [Range(0f,1f)]
    public float startingValue;

    private float value;
    private bool sliderHeld;

    private void Start()
    {
        sliderHeld = false;
        sliderSensor.OnDown += (go) => sliderHeld = true;
        sliderSensor.OnUp += (go) => sliderHeld = false;
        Value = startingValue;
    }

    private void Update()
    {
        if(sliderHeld)
        {
            var p = new Plane(lowEndTransform.position, new Vector3(0, 0, -1));
            var mouseRay = camera.ScreenPointToRay(Input.mousePosition);
            float d;
            if(p.Raycast(mouseRay, out d))
            {
                var mousePoint = mouseRay.GetPoint(d);
                Value = ProjectPointToLineAndClamp(lowEndTransform.position, highEndTransform.position, mousePoint);
            }
        }
    }

    private float ProjectPointToLineAndClamp(Vector3 a, Vector3 b, Vector3 p)
    {
        var ab = a - b;
        var ap = a - p;
        var result = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
        float t = Vector3.Dot(ab, result) / (ab.magnitude * ab.magnitude);
        t = Mathf.Clamp01(t);
        return t;
    }
}
