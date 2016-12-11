using UnityEngine;
using System;
using System.Collections.Generic;

// Manages the item the grid space holds
// Items stack so items should nest
[RequireComponent(typeof(GridTouch))]
public class GridContainer : MonoBehaviour
{
    public Action<GridItem> OnItemChanged;

    private GridItem content;

    private void Start()
    {
        GetComponent<GridTouch>().OnGridPressed += HandleTouch;
    }

    private void HandleTouch(GameObject obj)
    {
        if (content != null)
        {
            content.HandleTouch(obj);
        }
    }

    public void SetItem(GridItem item)
    {
        if(content != null)
            throw new System.Exception("Already an item here!");

        content = item;
        content.transform.SetParent(this.transform, false);
        content.transform.localPosition = Vector3.zero;
        content.transform.localRotation = Quaternion.identity;

        if (OnItemChanged != null) OnItemChanged(item);
    }

    public GridItem RemoveItem()
    {
        var temp = content;
        content.transform.SetParent(null, true);
        content = null;
        if (OnItemChanged != null) OnItemChanged(null);
        return temp;
    }

    public GridItem GetItem()
    {
        return content;
    }
}
