using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Load the model up from a player prefs or somewhere
public class LoadModelCommand : Command
{
    [Inject]
    public GameModel model { get; set; }

    

    public override void Execute()
    {
        if (JsonSavingUtility.Load(SaveModelCommand.SaveGameKey, model))
        {
            model.SuccessfulyLoaded = true;
        }
        else
        {
            model.SuccessfulyLoaded = false;
        }
    }
}
