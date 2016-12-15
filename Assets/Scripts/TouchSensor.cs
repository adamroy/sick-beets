using UnityEngine;
using System;

public class TouchSensor : MonoBehaviour
{
    public Action<GameObject> OnPressed;

    private void OnMouseUpAsButton()
    {
        if (OnPressed != null) OnPressed(this.gameObject);
    }
}
