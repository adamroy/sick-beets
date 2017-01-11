using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public class StoreModel
{
    [SerializeField]
    private List<string> unlockedItems;


    public StoreModel()
    {
        Clear();
    }

    public void Clear()
    {
        unlockedItems = new List<string>();
    }

    public void UnlockItem(StoreItem item)
    {
        if (unlockedItems.Contains(item.name) == false)
            unlockedItems.Add(item.name);
    }

    public void LockItem(StoreItem item)
    {
        unlockedItems.Remove(item.name);
    }

    public bool IsUnlocked(StoreItem item)
    {
        return unlockedItems.Contains(item.name);
    }
}