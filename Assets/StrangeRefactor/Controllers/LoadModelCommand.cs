using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Load the model up from a player prefs or somewhere
public class LoadModelCommand : Command
{
    [Inject]
    public ISickBeetsModel model { get; set; }

    public override void Execute()
    {
        if (JsonSavingUtility.Load("GameSave", model))
        {
            Debug.Log("Loadin success!");
        }
        else
        {
            Debug.Log("Creating model");
            // For now just fabricate the model from scratch until actual saving/loading
            var containerViews = GameObject.FindObjectsOfType<BeetContainerView>();
            foreach (var view in containerViews)
            {
                var containerModel = new BeetContainerModel();
                containerModel.InstanceID = view.GetInstanceID();
                containerModel.function = view.function;
                model.AddContainer(containerModel);
            }
        }
    }
}
