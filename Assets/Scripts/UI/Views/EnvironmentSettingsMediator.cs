using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class EnvironmentSettingsMediator : Mediator
{
    [Inject]
    public EnvironmentSettingsView view { get; set; }

    [Inject]
    public EnvironmentChangedSignal environmentChangedSignal { get; set; }

    [Inject]
    public ChangeActiveInputLayerSignal changeActiveInputLayerSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        view.OnSettingsChanged.AddListener(OnSliderChanged);
        view.OnPanelVisibleChanged.AddListener(OnSliderVisibleChanged);
    }

    private void OnSliderChanged(EnvironmentVariable need, float value)
    {
        environmentChangedSignal.Dispatch(need, value);
    }

    private void OnSliderVisibleChanged(bool visible)
    {
        changeActiveInputLayerSignal.Dispatch(InputLayer.UI, visible);
    }
}
