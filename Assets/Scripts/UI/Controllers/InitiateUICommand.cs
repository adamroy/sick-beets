﻿using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Takes the saved model and sends out appropriate signals to update UI to match
public class InitiateUICommand : Command
{
    [Inject]
    public AppModel model { get; set; }

    [Inject]
    public EnvironmentChangedSignal setEnvironmentVariable { get; set; }

    [Inject]
    public IEnvironmentVariableLibrary environmentVariableLibrary { get; set; }

    [Inject]
    public IStoreItemLibrary storeItemLibrary { get; set; }

    [Inject]
    public FundsChangedSignal fundsChangedSignal { get; set; }

    [Inject]
    public ResearchProgressChangedSignal researchProgressChangedSignal { get; set; }

    [Inject]
    public StoreItemUnlockedSignal storeItemUnlockedSignal { get; set; }

    [Inject]
    public StoreItemPurchasedSignal storeItemPurchasedSignal { get; set; }

    public override void Execute()
    {
        foreach(var variable in environmentVariableLibrary.EnvironmentVariables)
        {
            setEnvironmentVariable.Dispatch(variable, model.World.GetEnvironmentValue(variable));
        }

        fundsChangedSignal.Dispatch(model.GetFunds());
        researchProgressChangedSignal.Dispatch(model.Research.Progress);

        foreach (var item in storeItemLibrary.StoreItems)
        {
            if(model.Store.IsPurchased(item))
            {
                storeItemPurchasedSignal.Dispatch(item);
            }
            else if(model.Store.IsUnlocked(item))
            {
                storeItemUnlockedSignal.Dispatch(item);
            }
        }
    }
}
