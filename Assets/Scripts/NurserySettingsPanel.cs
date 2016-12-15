using UnityEngine;
using System.Collections;

public class NurserySettingsPanel : MonoBehaviour
{
    public TouchSensor touchSensor;

    // The actual panel, parent to all parts
    private GameObject panel;
    private Transform panelLoweredLocation, panelRaisedLocation;
    private bool active;

    private void Awake()
    {
        active = false;
        touchSensor.OnPressed += OnSensorTouched;
    }

    private void OnSensorTouched(GameObject sensor)
    {
        active = true;
    }

    private IEnumerator MovePanel(Transform target)
    {
        while(true)
        {
            yield return null;
        }
    }
}
