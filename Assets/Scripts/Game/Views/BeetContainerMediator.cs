﻿using UnityEngine;
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

    [Inject]
    public DisplayHealRateSignal displayHealRateSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        view.touchSignal.AddListener(TouchListener);

        if (view.function == BeetContainerFunction.Input)
            beetCreationSignal.AddListener(view.PlaceBeet);

        displayHealRateSignal.AddListener(DisplayHealRate);
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

    void DisplayHealRate(BeetContainerView container, float rate)
    {
        if(container == this.view)
        {
            // Since not all containers have this function, it's a separate component which we call through sendmessage
            this.view.SendMessage("DisplayHealingRate", rate, SendMessageOptions.DontRequireReceiver);
        }
    }
}
