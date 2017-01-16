using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public abstract class ItemMediator : Mediator
{
    public abstract IItemView itemView { get; }

    [Inject]
    public ActivateItemViewSignal activateItemViewSignal { get; set; }

    public override void OnRegister()
    {
        activateItemViewSignal.AddListener(OnActivateItem);
    }

    protected virtual void OnActivateItem(StoreItem item)
    {
        if(itemView.Item == item)
        {
            itemView.Activate();
        }
    }
}
