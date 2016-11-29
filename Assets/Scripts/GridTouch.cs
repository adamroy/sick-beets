using UnityEngine;
using System;

public class GridTouch : MonoBehaviour
{
    public Action<GameObject> OnGridPressed;

    private void OnMouseUpAsButton()
    {
        if (OnGridPressed != null) OnGridPressed(this.gameObject);
    }
}
