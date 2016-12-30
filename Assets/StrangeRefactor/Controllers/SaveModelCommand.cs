using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Save the model to playerprefs or somewhere
public class SaveModelCommand : Command
{
    // Since this is called from the pause signal
    [Inject]
    public bool pause { get; set; }

    [Inject]
    public SickBeetsModel model { get; set; }
    
    public override void Execute()
    {
        if (pause)
        {
            JsonSavingUtility.Save("GameSave", model);
        }
    }
}
