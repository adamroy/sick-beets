﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class EnvironmentSettingsView : View
{
    // The actual panel, parent to all parts
    public GameObject panel;
    public Transform panelLoweredLocation, panelRaisedLocation;
    public float raiseTime = 0.75f;
    public Signal<EnvironmentVariable, float> OnSettingsChanged = new Signal<EnvironmentVariable, float>();
    public Signal<bool> OnPanelVisibleChanged = new Signal<bool>();

    private bool active;
    private Dictionary<SliderView, EnvironmentVariable> sliderToEnvVariableMap;

    public void Init()
    {
        sliderToEnvVariableMap = new Dictionary<SliderView, EnvironmentVariable>();
        foreach (var slider in GetComponentsInChildren<SliderView>())
        {
            slider.Init();
            slider.OnValueChanged.AddListener(OnSliderChanged);
            var envVarAsgnmt = slider.GetComponent<EnvironmentVariableAssignmentView>();
            if (envVarAsgnmt == null)
                throw new Exception("Slider needs an EnvironmentVariableAssignmentView!");
            sliderToEnvVariableMap[slider] = envVarAsgnmt.variable;
        }
        active = false;
    }

    public void SetEnvironmentVariable(EnvironmentVariable variable, float value)
    {
        var sliders = GetComponentsInChildren<SliderView>();
        var targetSlider = sliders.FirstOrDefault(s => s.GetComponent<EnvironmentVariableAssignmentView>().variable == variable);
        if (targetSlider != null)
            targetSlider.SetValue(value);
    }

    private void OnSliderChanged(SliderView slider)
    {
        OnSettingsChanged.Dispatch(sliderToEnvVariableMap[slider], slider.Value);
    }

    private IEnumerator MovePanel(Transform target, bool enable)
    {
        var startPosition = panel.transform.position;
        if (enable) panel.SetActive(true);

        for (float t = 0, p = 0; t < raiseTime; t += Time.deltaTime, p = t / raiseTime)
        {
            panel.transform.position = Vector3.Lerp(startPosition, target.transform.position, p);
            yield return null;
        }

        if (!enable) panel.SetActive(false);
    }

    public void Toggle()
    {
        active = !active;
        if (active)
        {
            OnPanelVisibleChanged.Dispatch(true);
            StartCoroutine(MovePanel(panelRaisedLocation, true));
        }
        else
        {
            OnPanelVisibleChanged.Dispatch(false);
            StartCoroutine(MovePanel(panelLoweredLocation, false));
        }
    }
}
