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

    [Inject]
    public BeetModelUpdatedSignal beetModelUpdateSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        selectionSignal.AddListener(SelectionSignalListener);
        beetModelUpdateSignal.AddListener(BeetModelUpdateListener);
    }

    private void SelectionSignalListener(int instanceID)
    {
        if (view.GetInstanceID() == instanceID)
            view.MarkSelected();
        else
            view.MarkUnselected();
    }

    private void BeetModelUpdateListener(BeetModel model)
    {
        if (model.InstanceID == view.GetInstanceID())
        {
            view.SetHealthIndicator(model.Health);
        }
    }
}
