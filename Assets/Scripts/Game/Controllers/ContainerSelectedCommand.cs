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
    public AppModel model { get; set; }

    [Inject]
    public SelectBeetSignal beetSelectionSignal { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }
    
    [Inject]
    public RequestBeetCreationSignal beetCreationRequestSignal { get; set; }

    [Inject]
    public DestroyBeetSignal beetDestroySignal { get; set; }

    [Inject]
    public ResearchBeetSignal researchBeetSignal { get; set; }

    public override void Execute()
    {
        var containerModel = model.World.GetContainerByName(view.name);
        var beetModel = model.World.GetBeetAssignment(containerModel);

        if(beetModel != null)
        {
            if (model.World.SelectedBeet == beetModel) // If reselected, deselect
            {
                beetSelectionSignal.Dispatch(-1);
                model.World.SelectedBeet = null;
            }
            else 
            {
                beetSelectionSignal.Dispatch(beetModel.InstanceID);
                model.World.SelectedBeet = beetModel;
            }
        }
        else 
        {
            // If no beet at destination (and it's not the input) and we gave a selected beet, place that
            if (model.World.SelectedBeet != null && containerModel.Function != BeetContainerFunction.Input)
            {
                // If we are removing from the input, for now lets just generate another beet
                if(model.World.GetContainerByAssignment(model.World.SelectedBeet).Function == BeetContainerFunction.Input)
                    beetCreationRequestSignal.Dispatch();

                bool containerIsTransfer = containerModel.Function == BeetContainerFunction.LabTransfer;
                bool labHasBeet = model.World.GetBeetAssignment(model.World.GetContainerByFunction(BeetContainerFunction.Lab)) != null;
                if (!containerIsTransfer || (containerIsTransfer && !labHasBeet))
                {
                    model.World.AssignBeetToContainer(model.World.SelectedBeet, containerModel);
                    var beetView = Utils.GetBeetViewByModel(model.World.SelectedBeet); // GameObject.FindObjectsOfType<BeetView>().First(v => v.GetInstanceID() == model.World.SelectedBeet.InstanceID);
                    var containerView = Utils.GetBeetContainerViewByModel(containerModel); // GameObject.FindObjectsOfType<BeetContainerView>().First(v => v.name == containerModel.Name);
                    beetPlacementSignal.Dispatch(beetView, containerView);
                    beetSelectionSignal.Dispatch(-1); // Deselect

                    // Destroy beet if we are placing into output
                    if (containerModel.Function == BeetContainerFunction.Output)
                        beetDestroySignal.Dispatch(beetView, containerView, 2f);

                    // Transfer beet if we are placing into transfer container
                    if (containerModel.Function == BeetContainerFunction.LabTransfer)
                        researchBeetSignal.Dispatch(model.World.SelectedBeet);
                    
                    // Deselect now since this is needed 3 lines up
                    model.World.SelectedBeet = null;
                }
            }

        }
    }
}
