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
    public SickBeetsModel model { get; set; }

    // Create the beet view and model, connect them
    public override void Execute()
    {
        // Instantiate the gameobject, not just the view!
        var view = GameObject.Instantiate(beetLibrary.CommonBeetPrefab.gameObject).GetComponent<BeetView>();

        var beetModel = new BeetModel();
        beetModel.Type = BeetType.Common;
        beetModel.InstanceID = view.GetInstanceID();
        model.AddBeet(beetModel);
        var inputContainer = model.GetContainerByFunction(BeetContainerFunction.Input);
        model.AssignBeetToContainer(beetModel, inputContainer);

        // Let the world place the new beet where appropriate
        creationSignal.Dispatch(view);
    }
}
