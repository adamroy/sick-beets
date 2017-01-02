using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class TouchDetectorMediator : Mediator
{
    [Inject]
    public TouchDetectorView view { get; set; }

    [Inject]
    public SetInputLayerEnabledSignal setInputLayerEnabledSignal { get; set; }

    public override void OnRegister()
    {
        setInputLayerEnabledSignal.AddListener(EnableInputLayer);
    }

    private void EnableInputLayer(InputLayer layer, bool enabled)
    {
        if (view.InputLayer == layer)
        {
            view.SetTouchEnabled(enabled);
        }
    }
}
