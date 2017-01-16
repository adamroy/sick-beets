using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class ItemsWorldMediator : Mediator
{
    [Inject]
    public ItemsWorldView view { get; set; }

    [Inject]
    public ActivateItemViewSignal activateItemViewSignal { get; set; }

    public override void OnRegister()
    {
        activateItemViewSignal.AddListener(OnActivateItemView);
    }

    public void OnActivateItemView(StoreItem item)
    {
        view.ActivateItemView(item);
    }
}
