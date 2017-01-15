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
        var containerModel = model.GetContainerByName(view.name);
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
            if (model.SelectedBeet != null && containerModel.Function != BeetContainerFunction.Input)
            {
                // If we are removing from the input, for now lets just generate another beet
                if(model.GetContainerByAssignment(model.SelectedBeet).Function == BeetContainerFunction.Input)
                    beetCreationRequestSignal.Dispatch();

                bool containerIsTransfer = containerModel.Function == BeetContainerFunction.LabTransfer;
                bool labHasBeet = model.GetBeetAssignment(model.GetContainerByFunction(BeetContainerFunction.Lab)) != null;
                if (!containerIsTransfer || (containerIsTransfer && !labHasBeet))
                {
                    model.AssignBeetToContainer(model.SelectedBeet, containerModel);
                    var beetView = Utils.GetBeetViewByModel(model.SelectedBeet); // GameObject.FindObjectsOfType<BeetView>().First(v => v.GetInstanceID() == model.SelectedBeet.InstanceID);
                    var containerView = Utils.GetBeetContainerViewByModel(containerModel); // GameObject.FindObjectsOfType<BeetContainerView>().First(v => v.name == containerModel.Name);
                    beetPlacementSignal.Dispatch(beetView, containerView);
                    beetSelectionSignal.Dispatch(-1); // Deselect

                    // Destroy beet if we are placing into output
                    if (containerModel.Function == BeetContainerFunction.Output)
                        beetDestroySignal.Dispatch(beetView, containerView, 2f);

                    // Transfer beet if we are placing into transfer container
                    if (containerModel.Function == BeetContainerFunction.LabTransfer)
                        researchBeetSignal.Dispatch(model.SelectedBeet);
                    
                    // Deselect now since this is needed 3 lines up
                    model.SelectedBeet = null;
                }
            }

        }
    }
}
