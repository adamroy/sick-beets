using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Linq;

public class TransferToLabCommand : Command
{
    [Inject]
    public BeetModel beetModel { get; set; }
    
    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }

    [Inject]
    public GameModel model { get; set; }

    public override void Execute()
    {
        var containerModel = model.GetContainerByFunction(BeetContainerFunction.Lab);
        model.AssignBeetToContainer(beetModel, containerModel);

        var view = Utils.GetBeetViewByModel(beetModel); // GameObject.FindObjectsOfType<BeetView>().First(v => v.GetInstanceID() == beetModel.InstanceID);
        var labContainer = Utils.GetBeetContainerViewByFunction(BeetContainerFunction.Lab); // GameObject.FindObjectsOfType<BeetContainerView>().First(c => c.function == BeetContainerFunction.Lab);
        beetPlacementSignal.Dispatch(view, labContainer);
    }
}
