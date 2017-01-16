using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

// Collects all the view for purchased items into one view for easy access/interfacing
public class ItemsWorldView : View
{
    public void ActivateItemView(StoreItem item)
    {
        Debug.Log("Activating view for: " + item.name);
    }
}
