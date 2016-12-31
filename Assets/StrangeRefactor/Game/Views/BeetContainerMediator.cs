using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class BeetContainerMediator : Mediator
{
    [Inject]
    public BeetContainerView view { get; set; }

    [Inject]
    public BeetCreatedSignal beetCreationSignal { get; set; }

    [Inject]
    public ContainerSelectedSignal selectionSignal { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }
    
    public override void OnRegister()
    {
        view.Init();
        view.touchSignal.AddListener(TouchListener);

        if (view.function == BeetContainerFunction.Input)
            beetCreationSignal.AddListener(view.PlaceBeet);

        beetPlacementSignal.AddListener(PlaceBeet);
    }

    private void TouchListener()
    {
        selectionSignal.Dispatch(view);
    }

    void PlaceBeet(BeetView beet, BeetContainerView container)
    {
        if (container == this.view)
        {
            view.PlaceBeet(beet);
        }
    }
}
