using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class GeneticSequencerView : View
{
    public GameObject panel;
    public Transform panelLoweredLocation, panelRaisedLocation;
    public float raiseTime = 0.75f;
    public SequenceSliderView healthySlider;
    public SequenceSliderView unhealthySlider;
    public TouchDetectorView confirmButton;
    public TouchDetectorView cancelButton;

    private bool active = false;

    public void Init()
    {
        healthySlider.Init();
        unhealthySlider.Init();
        confirmButton.OnUpAsButtonSignal.AddListener(Confirm);
        cancelButton.OnUpAsButtonSignal.AddListener(Cancel);
    }

    private void Confirm()
    {

    }

    private void Cancel()
    {

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

    public void Toggle(SequencerData data)
    {
        active = data != null;
        if (active)
        {
            healthySlider.DisplaySequence(data.HealthySequence, 3, 1);
            unhealthySlider.DisplaySequence(data.UnhealthySequence, 3, 1);
            StartCoroutine(MovePanel(panelRaisedLocation, true));
        }
        else
        {
            StartCoroutine(MovePanel(panelLoweredLocation, false));
        }
    }
}
