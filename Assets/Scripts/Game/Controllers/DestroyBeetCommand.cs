﻿using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class DestroyBeetCommand : Command
{
    // The beet to destroy
    [Inject]
    public BeetView beetView { get; set; }

    // The container to remove it from
    [Inject]
    public BeetContainerView containerView { get; set; }

    // How many seconds to wait before obliterating the fella
    [Inject]
    public float delay { get; set; }

    [Inject]
    public FundsChangedSignal fundsChangedSignal { get; set; }

    [Inject]
    public AppModel model { get; set; }

    public override void Execute()
    {
        model.World.RemoveBeet(model.World.GetBeetByID(beetView.GetInstanceID()));
        // Get 100 funds from curing beet (TODO depends on beet health and rarity)
        model.MakeTransaction(100);
        fundsChangedSignal.Dispatch(model.GetFunds());
        GameObject.Destroy(beetView.gameObject, delay);
    }
}
