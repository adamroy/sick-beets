﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using strange.extensions.command.impl;
using strange.extensions.context.api;

// Takes a filled out model and instantiates the world from it.
public class InstantiateModelCommand : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    [Inject]
    public IBeetPrefabLibrary beetLibrary { get; set; }

    [Inject]
    public GameModel model { get; set; }

    [Inject]
    public PlaceBeetSignal beetPlacementSignal { get; set; }

    [Inject]
    public RequestBeetCreationSignal beetCreationRequestSignal { get; set; }

    public override void Execute()
    {
        if (model.SuccessfulyLoaded)
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
                    var containerView = containerViews.FirstOrDefault(cv => cv.name == containerModel.Name);

                    beetPlacementSignal.Dispatch(beetView, containerView);
                }
            }
        }
        // If the model wasn't loaded from a save file, then do some jazz for first run
        else
        {
            // Fabricate model 
            var containerViews = GameObject.FindObjectsOfType<BeetContainerView>();
            foreach (var view in containerViews)
            {
                var containerModel = new BeetContainerModel();
                containerModel.Name = view.name;
                containerModel.Function = view.function;
                model.AddContainer(containerModel);
            }

            // Add initial beet
            beetCreationRequestSignal.Dispatch();
        }
    }
}
