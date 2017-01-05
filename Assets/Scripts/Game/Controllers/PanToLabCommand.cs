using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class PanToLabCommand : Command
{
    [Inject]
    public PlaceCameraSignal placeCameraSignal { get; set; }

    [Inject]
    public SetInputLayerEnabledSignal setInputLayerEnabledSignal { get; set; }

    public override void Execute()
    {
        setInputLayerEnabledSignal.Dispatch(InputLayer.Camera, false);
        placeCameraSignal.Dispatch(CameraDestination.Lab);
    }
}