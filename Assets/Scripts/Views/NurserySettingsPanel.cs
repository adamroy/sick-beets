using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(NurserySettingsPanelModel))]
public class NurserySettingsPanel : MonoBehaviour, ScreenNavigator.InputConsumer
{
    public TouchSensor raiseTouchSensor, lowerTouchSensor;
    // The actual panel, parent to all parts
    public GameObject panel;
    public Transform panelLoweredLocation, panelRaisedLocation;
    public float raiseTime = 0.75f;

    public List<NeedSetter> settings;
    public event Action<Need, float> OnSettingsChanged
    {
        add
        {
            foreach (var setter in settings)
                setter.OnNeedSet += value;
        }
        remove
        {
            foreach (var setter in settings)
                setter.OnNeedSet -= value;
        }
    }

    private bool active;
    private NurserySettingsPanelModel model;

    private void Awake()
    {
        model = GetComponent<NurserySettingsPanelModel>();
        active = false;
        raiseTouchSensor.OnUpAsButton += OnSensorTouched;
        lowerTouchSensor.OnUpAsButton += OnSensorTouched;   
    }

    private void Start()
    {
        ScreenNavigator.Instance.AddInputConsumer(this);
    }

    private void OnSensorTouched(GameObject sensor)
    {
        if (sensor.GetComponent<TouchSensor>() == raiseTouchSensor)
        {
            active = true;
            StartCoroutine(MovePanel(panelRaisedLocation, true));
        }
        else
        {
            active = false;
            StartCoroutine(MovePanel(panelLoweredLocation, false));
        }
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

    public bool IsActive()
    {
        return active;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Menu))
        {
            if (active == false)
            {
                active = true;
                StartCoroutine(MovePanel(panelRaisedLocation, true));
            }
            else
            {
                active = false;
                StartCoroutine(MovePanel(panelLoweredLocation, false));
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            active = false;
            StartCoroutine(MovePanel(panelLoweredLocation, false));
        }
    }

}
