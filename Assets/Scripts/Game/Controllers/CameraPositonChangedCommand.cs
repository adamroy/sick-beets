using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CameraPositonChangedCommand : Command
{
    [Inject]
    public CameraDestination destination { get; set; }

    [Inject]
    public AppModel model { get; set; }

    public override void Execute()
    {
        model.World.SetCameraDestination(destination);
    }
}
