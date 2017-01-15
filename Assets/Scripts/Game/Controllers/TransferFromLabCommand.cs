using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;
using System.Linq;

public class TransferFromLabCommand : Command
{
    [Inject]
    public AppModel model { get; set; }

    [Inject]
    public PlaceBeetSignal placeBeetSignal { get; set; }

    public override void Execute()
    {
        var labContainerModel = model.GetContainerByFunction(BeetContainerFunction.Lab);
        var beet = model.GetBeetAssignment(labContainerModel);

        if (beet == null)
            throw new Exception("Lab should have a beet by this point!");

        var nurseryContainers = model.GetAllContainersByFunction(BeetContainerFunction.Nursery);
        var openNurseryContainer = nurseryContainers.First(c => model.GetBeetAssignment(c) == null);

        model.AssignBeetToContainer(beet, openNurseryContainer);

        var beetView = Utils.GetBeetViewByModel(beet);
        var containerView = Utils.GetBeetContainerViewByModel(openNurseryContainer);
        placeBeetSignal.Dispatch(beetView, containerView);
    }
}
