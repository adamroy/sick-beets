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
            else // Otherwise replace selection
            {
                beetSelectionSignal.Dispatch(beetModel.InstanceID);
                model.SelectedBeet = beetModel;
            }
        }
        else 
        {
            // If no beet at destination and we gave a selected beet, place that
            if (model.SelectedBeet != null)
            {
                model.AssignBeetToContainer(model.SelectedBeet, containerModel);
                var beetView = GameObject.FindObjectsOfType<BeetView>().First(v => v.GetInstanceID() == model.SelectedBeet.InstanceID);
                var containerView = GameObject.FindObjectsOfType<BeetContainerView>().First(v => v.GetInstanceID() == containerModel.InstanceID);
                beetPlacementSignal.Dispatch(beetView, containerView);
                beetSelectionSignal.Dispatch(-1); // Deselect
                model.SelectedBeet = null;
            }

        }
    }
}
