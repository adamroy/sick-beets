using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class EnvironmentSettingsMediator : Mediator
{
    [Inject]
    public EnvironmentSettingsView view { get; set; }

    public override void OnRegister()
    {
        view.Init();
        view.OnSettingsChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(Need need, float value)
    {
        
    }
}
