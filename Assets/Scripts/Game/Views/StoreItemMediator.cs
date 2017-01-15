using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class StoreItemMediator : Mediator
{
    [Inject]
    public StoreItemView view { get; set; }

    [Inject]
    public StoreItemSelectedSignal storeItemSelectedSignal { get; set; }

    [Inject]
    public StoreItemUnlockedSignal storeItemUnlockedSignal { get; set; }

    [Inject]
    public StoreItemPurchasedSignal storeItemPurchasedSignal { get; set; }

    public override void OnRegister()
    {
        storeItemUnlockedSignal.AddListener(OnItemUnlocked);
        storeItemPurchasedSignal.AddListener(OnItemPurchased);
        view.touchSignal.AddListener(OnTouch);
        view.Init();
    }

    private void OnTouch()
    {
        storeItemSelectedSignal.Dispatch(view);
    }

    private void OnItemUnlocked(StoreItem item)
    {
        if(view.item == item)
        {
            view.MarkUnlocked();
        }
    }

    private void OnItemPurchased(StoreItem item)
    {
        if(view.item == item)
        {
            view.MarkPurchased();
        }
    }
}
