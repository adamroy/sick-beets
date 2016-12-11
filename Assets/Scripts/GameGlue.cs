using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameGlue : MonoBehaviour
{
    public AutoGrid gridRoot;
    public GameObject beenPotPrefab;
    public GameObject beetPrefab;
    public GameObject providerPrefab;

    private const string timeKey = "GamePauseTime";
    private int placement = 0;

    private void Start()
    {
        gridRoot.GetAllAttached<GridContainer>().ForEach(c => c.OnItemChanged += RefreshGrid);
        gridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += GridTouched);
        StartCoroutine(MainCoroutine());
    }

    // Called when a new grid item is placed.  Recalculate what everything gets.
    private void RefreshGrid(GridItem item)
    {
        var providers = gridRoot.GetAllAttached<GridContainer>().Select(container => container.GetItem() as Provider).Where(i => i != null);
        var needsMet = new Dictionary<Need, float>();
        foreach(var p in providers)
        {
            if (!needsMet.ContainsKey(p.needMet))
                needsMet[p.needMet] = p.value;
            else
                needsMet[p.needMet] += p.value;
        }

        EventManager.Broadcast(EventManager.Event.NeedsMet, needsMet);
    }

    private void GridTouched(GameObject grid)
    {
        if (placement == 0)
        {
            var instance = Instantiate(beenPotPrefab).GetComponent<BeetPot>();
            grid.GetComponent<GridContainer>().SetItem(instance);
        }
        else if (placement == 1)
        {
            var pot = grid.GetComponent<GridContainer>().GetItem() as BeetPot;
            if (pot != null)
            {
                var instance = Instantiate(beetPrefab).GetComponent<Beet>();
                pot.SetBeet(instance);
                RefreshGrid(null);
            }
        }
        else if (placement == 2)
        {
            var instance = Instantiate(providerPrefab).GetComponent<Provider>();
            grid.GetComponent<GridContainer>().SetItem(instance);
        }
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
            if (Input.GetKeyDown(KeyCode.I)) placement = 0;
            if (Input.GetKeyDown(KeyCode.O)) placement = 1;
            if (Input.GetKeyDown(KeyCode.P)) placement = 2;

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
