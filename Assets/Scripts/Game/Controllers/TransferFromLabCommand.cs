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
        var labContainerModel = model.World.GetContainerByFunction(BeetContainerFunction.Lab);
        var beet = model.World.GetBeetAssignment(labContainerModel);

        if (beet == null)
            throw new Exception("Lab should have a beet by this point!");

        var nurseryContainers = model.World.GetAllContainersByFunction(BeetContainerFunction.Nursery);
        var openNurseryContainer = nurseryContainers.First(c => model.World.GetBeetAssignment(c) == null);

        model.World.AssignBeetToContainer(beet, openNurseryContainer);

        var beetView = Utils.GetBeetViewByModel(beet);
        var containerView = Utils.GetBeetContainerViewByModel(openNurseryContainer);
        placeBeetSignal.Dispatch(beetView, containerView);
    }
}
