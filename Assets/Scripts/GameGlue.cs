using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BeetGenerator))]
public class GameGlue : MonoBehaviour
{
    public AutoGrid nurseryGridRoot;
    public AutoGrid intakeGridRoot;
    public AutoGrid labGridRoot;
    public AutoGrid releaseGrid;
    public NurserySettingsPanel settingsPanel;

    private BeetGenerator generator;
    private const string timeKey = "GamePauseTime";
    private Beet selectedBeet;
    private Dictionary<Need, float> needsMet;

    private void Start()
    {
        needsMet = new Dictionary<Need, float>();
        generator = GetComponent<BeetGenerator>();
        nurseryGridRoot.GetAllAttached<TouchSensor>().ForEach(t => t.OnUpAsButton += NurseryGridTouched);
        intakeGridRoot.GetAllAttached<TouchSensor>().ForEach(t => t.OnUpAsButton += IntakeGridTouched);
        labGridRoot.GetAllAttached<TouchSensor>().ForEach(t => t.OnUpAsButton += LabGridTouched);
        releaseGrid.GetComponent<TouchSensor>().OnUpAsButton += ReleaseGridTouched;
        settingsPanel.OnSettingsChanged += SettingsChanged;
        StartCoroutine(MainCoroutine());
    }

    private void SettingsChanged(Need need, float value)
    {
        needsMet[need] = value;

        foreach(var container in nurseryGridRoot.GetAllAttached<BeetContainer>())
        {
            if(!container.IsEmpty)
            {
                container.Beet.SetNeedsMet(needsMet);
            }
        }
    }

    private void NurseryGridTouched(GameObject grid)
    {
        var container = grid.GetComponent<BeetContainer>();
        if (container == null) return;

        // If empty and we have a selected beet, place
        // If not empty and no selected beet, select that beet
        // If not empty and we have a selected beet, nothing
        // Empty and no beet, nothing
        if (container.IsEmpty)
        {
            if (selectedBeet != null)
            {
                selectedBeet.RemoveFromContainer();
                selectedBeet.MarkUnselected();
                container.SetBeet(selectedBeet);
                selectedBeet.SetNeedsMet(needsMet);
                selectedBeet = null;
            }
        }
        else
        {
            var destinationBeet = container.Beet;
            if(destinationBeet == selectedBeet)
            {
                selectedBeet.MarkUnselected();
                selectedBeet = null;
            }
            else if(selectedBeet == null)
            {
                selectedBeet = container.Beet;
                selectedBeet.MarkSelected();
            }
        }
    }

    private void IntakeGridTouched(GameObject grid)
    {
        var beetContainer = grid.GetComponent<BeetContainer>();
        if (beetContainer == null) return;
        var beet = beetContainer.Beet;
        
        // If not empty select that beet
        // If empty or we re-select beet, unselect
        if ((selectedBeet == beet && selectedBeet != null) || (beet == null && selectedBeet != null))
        {
            selectedBeet.MarkUnselected();
            selectedBeet = null;
            return;
        }
        else if(beet != null)
        {
            if (selectedBeet != null)
                selectedBeet.MarkUnselected();
            beet.MarkSelected();
            selectedBeet = beet;
        }
    }

    private void LabGridTouched(GameObject grid)
    {
        var container = grid.GetComponent<BeetContainer>();
        if (container == null) return;

        // If empty and we have a selected beet, place
        // If not empty and no selected beet, select that beet
        // If not empty and we have a selected beet, nothing
        // Empty and no beet, nothing
        if (container.IsEmpty)
        {
            if (selectedBeet != null)
            {
                selectedBeet.RemoveFromContainer();
                selectedBeet.MarkUnselected();
                container.SetBeet(selectedBeet);
                // selectedBeet.SetNeedsMet(needsMet); /**** Same as Nursery action for now except we don't get any needs met ****/
                selectedBeet = null;
            }
        }
        else
        {
            var destinationBeet = container.Beet;
            if (destinationBeet == selectedBeet)
            {
                selectedBeet.MarkUnselected();
                selectedBeet = null;
            }
            else if (selectedBeet == null)
            {
                selectedBeet = container.Beet;
                selectedBeet.MarkSelected();
            }
        }
    }

    private void ReleaseGridTouched(GameObject grid)
    {
        print("Here");
        var container = grid.GetComponent<BeetContainer>();
        if (container == null || selectedBeet == null) return;
        print("Here2");
        if (selectedBeet.IsHealed == false) return;
        print("Here3");
        if (container.IsEmpty == false) return;
        print("Here4");

        selectedBeet.RemoveFromContainer();
        container.SetBeet(selectedBeet);
        Invoke("ReleaseBeet", 3f);
    }

    private void ReleaseBeet()
    {
        var container = releaseGrid.GetComponent<BeetContainer>();
        if (container.IsEmpty) return;

        var beet = container.RemoveBeet();
        Destroy(beet);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveTime();
    }

    public void OnApplicationQuit()
    {
        SaveTime();
    }

    private IEnumerator MainCoroutine()
    {
        LoadTimeAndUpdate();

        while (true)
        {
            int delta = Mathf.RoundToInt(Time.deltaTime * 1000);
            UpdateWorld(delta);
            yield return null;
        }
    }

    private void SaveTime()
    {
        // Save time in seconds, though we work with millis out here
        PlayerPrefs.SetInt(timeKey, CurrentSeconds());
        PlayerPrefs.Save();
    }

    private void LoadTimeAndUpdate()
    {
        // Save time in seconds, though we work with millis out here
        int previousTime = PlayerPrefs.GetInt(timeKey, CurrentSeconds());
        int delta = CurrentSeconds() - previousTime;
        UpdateWorld(delta * 1000); // Expects milliseconds
    }

    private void UpdateWorld(int deltaMillis)
    {
        EventManager.Broadcast(EventManager.Event.UpdateSimulation, deltaMillis);
    }
    
    // Seconds since UTC epoch
    private int CurrentSeconds()
    {
        return (int)((DateTime.UtcNow.Ticks - 621355968000000000) / TimeSpan.TicksPerSecond);
    }
}
