using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Linq;

public class TransferToLabCommand : Command
{
    [Inject]
    public BeetView view { get; set; }

    [Inject]
    public float delay { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }

    [Inject]
    public SickBeetsModel model { get; set; }

    public override void Execute()
    {
        Retain();
        view.StartCoroutine(TransferAfterDelay());
    }

    private IEnumerator TransferAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        var labContainer = GameObject.FindObjectsOfType<BeetContainerView>().First(container => container.function == BeetContainerFunction.Lab);

        var beetModel = model.GetBeetByID(view.GetInstanceID());
        var containerModel = model.GetContainerByID(labContainer.GetInstanceID());
        model.AssignBeetToContainer(beetModel, containerModel);

        beetPlacementSignal.Dispatch(view, labContainer);
        Release();
    }
}
