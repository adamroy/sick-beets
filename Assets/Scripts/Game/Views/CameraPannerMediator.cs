using UnityEngine;
using System.Collections;
using System;

public class CameraPannerMediator : InputEnablerMediator
{
    [Inject]
    public CameraPannerView view { get; set; }

    [Inject]
    public PlaceCameraSignal placeCameraSignal { get; set; }

    protected override IInputEnabler View { get { return view; } }

    public override void OnRegister()
    {
        base.OnRegister();
        placeCameraSignal.AddListener(OnPlaceCamera);
    }

    private void OnPlaceCamera(CameraDestination dest)
    {
        view.MoveToDestinarion(dest);
    }
}
