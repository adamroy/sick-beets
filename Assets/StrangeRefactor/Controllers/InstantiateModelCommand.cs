using UnityEngine;
using System.Collections;
using System.Linq;
using strange.extensions.command.impl;

// Takes a filled out model and instantiates the world from it.
public class InstantiateModelCommand : Command
{
    [Inject]
    public IBeetPrefabLibrary beetLibrary { get; set; }

    [Inject]
    public SickBeetsModel model { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }

    public override void Execute()
    {
        var containerViews = GameObject.FindObjectsOfType<BeetContainerView>();

        foreach (var kvp in model.GetAllAssignements())
        {
            var containerModel = kvp.Key;
            var beetModel = kvp.Value;

            // Instantiate the gameobject, not just the view!
            if (beetModel.Type == BeetType.Common)
            {
                var beetView = GameObject.Instantiate(beetLibrary.CommonBeetPrefab.gameObject).GetComponent<BeetView>();
                beetModel.InstanceID = beetView.GetInstanceID();
                var containerView = containerViews.FirstOrDefault(cv => cv.GetInstanceID() == containerModel.InstanceID);

                beetPlacementSignal.Dispatch(beetView, containerView);
            }
        }
    }
}
