using UnityEngine;
using System;

public class TouchSensor : MonoBehaviour
{
    public Action<GameObject> OnUpAsButton;
    public Action<GameObject> OnDown;
    public Action<GameObject> OnUp;

    private void OnMouseUpAsButton()
    {
        if (OnUpAsButton != null) OnUpAsButton(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (OnDown != null) OnDown(this.gameObject);
    }

    public void OnMouseUp()
    {
        if (OnUp != null) OnUp(this.gameObject);
    }
}
