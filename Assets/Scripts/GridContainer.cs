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
        content.HandleTouch(obj);
    }

    public void SetItem(GridItem item)
    {
        content = item;
    }

    public GridItem RemoveItem()
    {
        var temp = content;
        content = null;
        return temp;
    }
}
