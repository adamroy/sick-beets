using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class StoreItemView : View
{
    public StoreItem item;

    public Signal touchSignal = new Signal();

    public void Init()
    {
        var pd = gameObject.AddComponent<TouchDetectorView>();
        pd.InputLayer = InputLayer.Game;
        pd.OnUpAsButtonSignal.AddListener(() => touchSignal.Dispatch());
    }

    public void MarkUnlocked()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void MarkPurchased()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}
