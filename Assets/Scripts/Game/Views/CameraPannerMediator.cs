using UnityEngine;
using System.Collections;
using System;

public class CameraPannerMediator : InputEnablerMediator
{
    [Inject]
    public CameraPannerView view { get; set; }

    [Inject]
    public PlaceCameraSignal placeCameraSignal { get; set; }

    [Inject]
    public CameraPositionChangedSignal cameraPositionChangedSignal { get; set; }

    protected override IInputEnabler View { get { return view; } }

    public override void OnRegister()
    {
        base.OnRegister();
        placeCameraSignal.AddListener(OnPlaceCamera);
        view.OnCameraPositonChanged.AddListener(OnCameraPositionChanged);
    }

    private void OnPlaceCamera(CameraDestination dest, bool withLerping)
    {
        view.MoveToDestination(dest, withLerping);
    }

    private void OnCameraPositionChanged(CameraDestination dest)
    {
        cameraPositionChangedSignal.Dispatch(dest);
    }
}
