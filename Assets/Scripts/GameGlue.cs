using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGlue : MonoBehaviour
{
    public AutoGrid gridRoot;
    public GameObject beenPotPrefab;

    private const string timeKey = "GamePauseTime";

    private void Start()
    {
        gridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += GridTouched);
        StartCoroutine(MainCoroutine());
    }

    private void GridTouched(GameObject grid)
    {
        var instance = Instantiate(beenPotPrefab).GetComponent<BeetPot>();
        grid.GetComponent<GridContainer>().SetItem(instance);
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
