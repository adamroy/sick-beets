using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Takes a filled out model and instantiates the world from it.
public class InstantiateModelCommand : Command
{
    [Inject]
    public ISickBeetsModel model { get; set; }

    public override void Execute()
    {
        
    }
}
