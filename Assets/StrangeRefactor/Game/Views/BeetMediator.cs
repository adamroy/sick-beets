using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class BeetMediator : Mediator
{
    [Inject]
    public BeetView view { get; set; }

    [Inject]
    public SelectBeetSignal selectionSignal { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        selectionSignal.AddListener(SelectionSignalListener);
    }

    private void SelectionSignalListener(int instanceID)
    {
        if (view.GetInstanceID() == instanceID)
            view.MarkSelected();
        else
            view.MarkUnselected();
    }
}
