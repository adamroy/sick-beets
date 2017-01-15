using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CreateBeetCommand : Command
{
    [Inject]
    public IBeetPrefabLibrary beetLibrary { get; set; }

    [Inject]
    public BeetCreatedSignal creationSignal { get; set; }

    [Inject]
    public AppModel model { get; set; }

    // Create the beet view and model, connect them
    public override void Execute()
    {
        // Instantiate the gameobject, not just the view!
        var view = GameObject.Instantiate(beetLibrary.CommonBeetPrefab.gameObject).GetComponent<BeetView>();

        var beetModel = new BeetModel();
        beetModel.Type = BeetType.Common;
        beetModel.InstanceID = view.GetInstanceID();
        beetModel.LifeSpan = 10f; // 10 seconds
        foreach (var need in view.environmentNeeds)
            beetModel.AddEnvironmentNeed(need.variable, need.value);
        model.World.AddBeet(beetModel);
        var inputContainer = model.World.GetContainerByFunction(BeetContainerFunction.Input);
        model.World.AssignBeetToContainer(beetModel, inputContainer);

        // Let the world place the new beet where appropriate
        creationSignal.Dispatch(view);
    }
}
