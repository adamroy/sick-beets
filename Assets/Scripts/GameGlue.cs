using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(BeetGenerator))]
public class GameGlue : MonoBehaviour
{
    public AutoGrid nurseryGridRoot;
    public AutoGrid intakeGridRoot;
    public GameObject beenPotPrefab;
    public GameObject beetPrefab;
    public GameObject providerPrefab;

    private BeetGenerator generator;
    private const string timeKey = "GamePauseTime";
    private int placement = 0;
    private Beet selectedBeet;

    private void Start()
    {
        generator = GetComponent<BeetGenerator>();
        nurseryGridRoot.GetAllAttached<GridContainer>().ForEach(c => c.OnItemChanged += RefreshGrid);
        nurseryGridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += NurseryGridTouched);
        intakeGridRoot.GetAllAttached<GridTouch>().ForEach(t => t.OnGridPressed += IntakeGridTouched);
        StartCoroutine(MainCoroutine());
    }

    // Called when a new grid item is placed.  Recalculate what everything gets.
    private void RefreshGrid(GridItem item)
    {
        var providers = nurseryGridRoot.GetAllAttached<GridContainer>().Select(container => container.GetItem() as Provider).Where(i => i != null);
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

    private void IntakeGridTouched(GameObject grid)
    {
        var beetPot = grid.GetComponent<BeetPot>();
        if (beetPot == null)
            return;

        var beet = beetPot.GetBeet();
        if (beet == null)
            return;

        if (selectedBeet == beet)
        {
            beet.MarkUnSelected();
            selectedBeet = null;
            return;
        }
        else
        {
            beet.MarkSelected();
            selectedBeet = beet;
        }
    }

    private void NurseryGridTouched(GameObject grid)
    {
        // Place beet pot
        if (placement == 0)
        {
            var instance = Instantiate(beenPotPrefab).GetComponent<BeetPot>();
            grid.GetComponent<GridContainer>().SetItem(instance);
        }
        // Place beet
        else if (placement == 1) 
        {
            var pot = grid.GetComponent<GridContainer>().GetItem() as BeetPot;
            if (pot != null && selectedBeet != null)
            {
                selectedBeet.transform.parent.GetComponent<BeetPot>().RemoveBeet();
                selectedBeet.MarkUnSelected();
                pot.SetBeet(selectedBeet);
                RefreshGrid(null);
            }
        }
        // Place provider
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
