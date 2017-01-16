using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class StoreItemPurchasedCommand : Command
{
    [Inject]
    public StoreItem itemPurchased { get; set; }

    [Inject]
    public AppModel model { get; set; }

    [Inject]
    public RuntimeCofiguration runtimeCofiguration { get; set; }

    [Inject]
    public ActivateItemViewSignal activateItemViewSignal { get; set; }

    public override void Execute()
    {
        model.Store.PurchaseItem(itemPurchased);
        itemPurchased.effect.Apply(runtimeCofiguration);
        activateItemViewSignal.Dispatch(itemPurchased);
    }
}
