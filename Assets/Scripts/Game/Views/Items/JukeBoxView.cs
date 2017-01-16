using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class JukeBoxView : View, IItemView
{
    [SerializeField]
    private StoreItem item;
    public StoreItem Item { get { return item; } }

    public GameObject model;

    public void Activate()
    {
        model.SetActive(true);
    }

    public void Deactivate()
    {
        model.SetActive(false);
    }
}
