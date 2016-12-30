using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Linq;

// Transfers beets between containers, manages selected beet
public class ContainerSelectedCommand : Command
{
    [Inject]
    public BeetContainerView view { get; set; }

    [Inject]
    public ISickBeetsModel model { get; set; }

    [Inject]
    public SelectBeetSignal beetSelectionSignal { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }
    
    [Inject]
    public RequestBeetCreationSignal beetCreationRequestSignal { get; set; }

    [Inject]
    public DestroyBeetSignal beetDestroySignal { get; set; }

    [Inject]
    public TransferToLabSignal labTransferSignal { get; set; }

    public override void Execute()
    {
        var containerModel = model.GetContainerByID(view.GetInstanceID());
        var beetModel = model.GetBeetAssignment(containerModel);

        if(beetModel != null)
        {
            if (model.SelectedBeet == beetModel) // If reselected, deselect
            {
                beetSelectionSignal.Dispatch(-1);
                model.SelectedBeet = null;
            }
            else 
            {
                beetSelectionSignal.Dispatch(beetModel.InstanceID);
                model.SelectedBeet = beetModel;
            }
        }
        else 
        {
            // If no beet at destination (and it's not the input) and we gave a selected beet, place that
            if (model.SelectedBeet != null && containerModel.function != BeetContainerFunction.Input)
            {
                // If we are removing from the input, for now lets just generate another beet
                if(model.GetContainerAssignment(model.SelectedBeet).function == BeetContainerFunction.Input)
                    beetCreationRequestSignal.Dispatch();

                bool containerIsTransfer = containerModel.function == BeetContainerFunction.LabTransfer;
                bool labHasBeet = model.GetBeetAssignment(model.GetContainerByFunction(BeetContainerFunction.Lab)) != null;
                if (!containerIsTransfer || (containerIsTransfer && !labHasBeet))
                {
                    model.AssignBeetToContainer(model.SelectedBeet, containerModel);
                    var beetView = GameObject.FindObjectsOfType<BeetView>().First(v => v.GetInstanceID() == model.SelectedBeet.InstanceID);
                    var containerView = GameObject.FindObjectsOfType<BeetContainerView>().First(v => v.GetInstanceID() == containerModel.InstanceID);
                    beetPlacementSignal.Dispatch(beetView, containerView);
                    beetSelectionSignal.Dispatch(-1); // Deselect
                    model.SelectedBeet = null;

                    // Destroy beet if we are placing into output
                    if (containerModel.function == BeetContainerFunction.Output)
                        beetDestroySignal.Dispatch(beetView, containerView, 2f);
                    // Transfer beet if we are placing into transfer container
                    if (containerModel.function == BeetContainerFunction.LabTransfer)
                        labTransferSignal.Dispatch(beetView, 2f);
                }
            }

        }
    }
}
