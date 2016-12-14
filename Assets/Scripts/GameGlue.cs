﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BeetGenerator))]
public class GameGlue : MonoBehaviour
{
    public AutoGrid nurseryGridRoot;
    public AutoGrid intakeGridRoot;
    public AutoGrid labGridRoot;

    private BeetGenerator generator;
    private const string timeKey = "GamePauseTime";
    private Beet selectedBeet;

    private void Start()
    {
        generator = GetComponent<BeetGenerator>();
        nurseryGridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += NurseryGridTouched);
        intakeGridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += IntakeGridTouched);
        labGridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += IntakeGridTouched);
        StartCoroutine(MainCoroutine());
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
                selectedBeet.transform.parent.GetComponent<BeetContainer>().RemoveBeet();
                selectedBeet.MarkUnselected();
                container.SetBeet(selectedBeet);
                selectedBeet = null;
            }
        }
        else
        {
            if(selectedBeet == null)
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
        // For now same action
        NurseryGridTouched(grid);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveTime();
        // else LoadTimeAndUpdate();
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
