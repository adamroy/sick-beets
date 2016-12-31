using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Load the model up from a player prefs or somewhere
public class LoadModelCommand : Command
{
    [Inject]
    public GameModel model { get; set; }

    [Inject]
    public RequestBeetCreationSignal beetCreationRequestSignal { get; set; }

    public override void Execute()
    {
        if (JsonSavingUtility.Load(SaveModelCommand.SaveGameKey, model))
        {
            model.SuccessfulyLoaded = true;
        }
        else
        {
            model.SuccessfulyLoaded = false;

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
