using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public class StoreModel
{
    [SerializeField]
    private List<string> unlockedItems;

    [SerializeField]
    private List<string> purchasedItems;

    public StoreModel()
    {
        Clear();
    }

    public void Clear()
    {
        unlockedItems = new List<string>();
        purchasedItems = new List<string>();
    }

    #region public data methods

    public void UnlockItem(StoreItem item)
    {
        if (!unlockedItems.Contains(item.name))
            unlockedItems.Add(item.name);
    }

    public bool IsUnlocked(StoreItem item)
    {
        return unlockedItems.Contains(item.name);
    }

    public void PurchaseItem(StoreItem item)
    {
        if (!purchasedItems.Contains(item.name))
            purchasedItems.Add(item.name);
    }

    public bool IsPurchased(StoreItem item)
    {
        return purchasedItems.Contains(item.name);
    }

    #endregion
}