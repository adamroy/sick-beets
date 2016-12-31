using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class BeetContainerView : View
{
    public Transform targetPosition;
    public BeetContainerFunction function;

    public Signal touchSignal = new Signal();

    public void Init()
    {
        var pd = gameObject.AddComponent<TouchDetector>();
        pd.OnUpAsButtonSignal.AddListener(() => touchSignal.Dispatch());
    }

    public void PlaceBeet(BeetView beet)
    {
        beet.transform.SetParent(this.transform, false);
        beet.transform.position = targetPosition.position;
    }
}
