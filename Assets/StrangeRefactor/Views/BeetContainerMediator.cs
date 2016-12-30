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
        view.touchSignal.AddListener(
            () => selectionSignal.Dispatch(view));

        if (view.function == BeetContainerFunction.Input)
            beetCreationSignal.AddListener(view.PlaceBeet);

        beetPlacementSignal.AddListener(
            (beet, container) =>
            {
                if (container == this.view)
                {
                    view.PlaceBeet(beet);
                }
            });
    }
}
