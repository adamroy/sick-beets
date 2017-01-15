using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class StoreDisplayView : View
{
    // The actual panel, parent to all parts
    public GameObject panel;
    public Transform panelLoweredLocation, panelRaisedLocation;
    public float raiseTime = 0.75f;
    public TextMesh displayText;
    public TouchDetectorView cancelButton;
    public TouchDetectorView buyButton;

    public Signal CancelSignal = new Signal();
    public Signal BuySignal = new Signal();

    public void Init()
    {
        cancelButton.OnUpAsButtonSignal.AddListener(Cancel);
        buyButton.OnUpAsButtonSignal.AddListener(Buy);
    }

    private void Cancel()
    {
        CancelSignal.Dispatch();
    }

    private void Buy()
    {
        BuySignal.Dispatch();
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

    public void DisplayItem(StoreItem item)
    {
        StartCoroutine(MovePanel(panelRaisedLocation, true));
        displayText.text = item.description;
    }

    public void Hide()
    {
        StartCoroutine(MovePanel(panelLoweredLocation, false));
    }
}
