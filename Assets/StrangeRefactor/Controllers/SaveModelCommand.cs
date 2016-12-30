using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Save the model to playerprefs or somewhere
public class SaveModelCommand : Command
{
    [Inject]
    public ISickBeetsModel model { get; set; }

    public override void Execute()
    {
        
    }
}
