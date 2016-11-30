using UnityEngine;
using System.Collections.Generic;

// Manages the item the grid space holds
// Items stack so items should nest
[RequireComponent(typeof(GridTouch))]
public class GridContainer : MonoBehaviour
{
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
    }

    public GridItem RemoveItem()
    {
        var temp = content;
        content.transform.SetParent(null, true);
        content = null;
        return temp;
    }
}
