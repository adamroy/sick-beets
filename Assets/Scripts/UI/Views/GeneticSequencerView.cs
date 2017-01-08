using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using System.Linq;
using strange.extensions.signal.impl;

public class GeneticSequencerView : View
{
    public Signal<List<Base>> OnValidSequenceConfirmed = new Signal<List<Base>>();
    public Signal OnCancel = new Signal();

    public GameObject panel;
    public Transform panelLoweredLocation, panelRaisedLocation;
    public float raiseTime = 0.75f;
    public SequenceSliderView healthySlider;
    public SequenceSliderView unhealthySlider;
    public TouchDetectorView confirmButton;
    public TouchDetectorView cancelButton;
    public new Camera camera;

    private bool active = false;

    public void Init()
    {
        healthySlider.Init();
        unhealthySlider.Init();
        confirmButton.RaycastCamera = camera;
        cancelButton.RaycastCamera = camera;
        confirmButton.OnUpAsButtonSignal.AddListener(Confirm);
        cancelButton.OnUpAsButtonSignal.AddListener(Cancel);
    }

    private void PrintSequence(List<Base> seq)
    {
        string seqStr = "";
        foreach (var b in seq)
            seqStr += b.name;
        print(seqStr);
    }

    private void Confirm()
    {
        var hlm = healthySlider.GetLeftMargin();
        var ulm = unhealthySlider.GetLeftMargin();

        //print("Helthy Left Margin");
        //PrintSequence(hlm);
        //print("Unhealth Left Margin");
        //PrintSequence(ulm);

        var hrm = healthySlider.GetRightMargin();
        var urm = unhealthySlider.GetRightMargin();

        //print("Helthy Right Margin");
        //PrintSequence(hrm);
        //print("Unhealth Right Margin");
        //PrintSequence(urm);

        if (hlm.SequenceEqual(ulm) && hrm.SequenceEqual(urm))
        {
            var researchSelection = unhealthySlider.GetSelectedSequence();
            OnValidSequenceConfirmed.Dispatch(researchSelection);

            //print("Helthy Selection");
            //PrintSequence(healthySlider.GetSelectedSequence());
            //print("Unhealthy Selection");
            //PrintSequence(researchSelection);
            //print("Valid alignment");
        }
        else
        {
            //print("Invalid alignment");
            // Handle selection of invalid sequence
            // (Display visual signal that it is invalid)
        }
    }

    private void Cancel()
    {
        OnCancel.Dispatch();
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

    public void Display(SequencerData data)
    {
        active = true;
        healthySlider.DisplaySequence(data.HealthySequence, 3, 1);
        unhealthySlider.DisplaySequence(data.UnhealthySequence, 3, 1);
        StartCoroutine(MovePanel(panelRaisedLocation, true));
    }

    public void Hide()
    {
        active = false;
        healthySlider.ClearSequence();
        unhealthySlider.ClearSequence();
        StartCoroutine(MovePanel(panelLoweredLocation, false));
    }
}
