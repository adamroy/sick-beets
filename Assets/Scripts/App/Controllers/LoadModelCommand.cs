using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using strange.extensions.context.api;

// Load the model up from a player prefs or somewhere
public class LoadModelCommand : Command
{
    [Inject]
    public AppModel model { get; set; }
    
    public override void Execute()
    {
        if (JsonSavingUtility.Load(SaveModelCommand.SaveGameKey, model))
        {
            model.SuccessfulyLoaded = true;
        }
        else
        {
            // We may have botched the model at this point, so clear it
            model.Clear();
            model.SuccessfulyLoaded = false;
        }
    }
}
