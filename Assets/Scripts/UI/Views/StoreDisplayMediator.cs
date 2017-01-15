using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class StoreDisplayMediator : Mediator
{
    [Inject]
    public StoreDisplayView view { get; set; }

    [Inject]
    public ChangeActiveInputLayerSignal changeActiveInputLayerSignal { get; set; }

    [Inject]
    public StoreItemSelectedSignal storeItemSelectedSignal { get; set; }

    [Inject]
    public StoreItemPurchasedSignal storeItemPurchasedSignal { get; set; }

    private StoreItem itemDisplayed;

    public override void OnRegister()
    {
        storeItemSelectedSignal.AddListener(DisplayItem);
        view.CancelSignal.AddListener(OnCancel);
        view.BuySignal.AddListener(OnBuy);
        view.Init();
    }

    private void DisplayItem(StoreItemView itemView)
    {
        itemDisplayed = itemView.item;
        view.DisplayItem(itemDisplayed);
        changeActiveInputLayerSignal.Dispatch(InputLayer.UI, true);
    }

    private void OnCancel()
    {
        view.Hide();
        itemDisplayed = null;
        changeActiveInputLayerSignal.Dispatch(InputLayer.UI, false);
    }

    private void OnBuy()
    {
        view.Hide();
        storeItemPurchasedSignal.Dispatch(itemDisplayed);
        itemDisplayed = null;
        changeActiveInputLayerSignal.Dispatch(InputLayer.UI, false);
    }
}
