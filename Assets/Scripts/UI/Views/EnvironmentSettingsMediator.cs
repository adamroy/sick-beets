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

    [Inject]
    public ToggleEnvironmentSettingsPanelSignal toggleEnvironmentSettingsPanelSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        view.OnSettingsChanged.AddListener(OnSliderChanged);
        view.OnPanelVisibleChanged.AddListener(OnSliderVisibleChanged);

        // This signal is used as a two way channel, so we can get commands to set vars from here
        environmentChangedSignal.AddListener(OnEnvironmentChanged);

        toggleEnvironmentSettingsPanelSignal.AddListener(OnTogglePanel);
    }

    private void OnSliderChanged(EnvironmentVariable need, float value)
    {
        environmentChangedSignal.Dispatch(need, value);
    }

    private void OnSliderVisibleChanged(bool visible)
    {
        changeActiveInputLayerSignal.Dispatch(InputLayer.UI, visible);
    }

    private void OnEnvironmentChanged(EnvironmentVariable var, float value)
    {
        view.SetEnvironmentVariable(var, value);
    }

    private void OnTogglePanel()
    {
        view.Toggle();
    }
}
