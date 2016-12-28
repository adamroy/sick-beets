using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameGlue : MonoBehaviour, ScreenNavigator.InputConsumer, JsonModel
{
    public AutoGrid nurseryGridRoot;
    public AutoGrid labGridRoot;
    public AutoGrid releaseGrid;
    public AutoGrid catchGrid;
    public AutoGrid labGrid;
    public NurserySettingsPanel settingsPanel;
    public Beet beetPrefab;
    
    private const string timeKey = "GamePauseTime";
    private const string gameModelKey = "GameData";
    private Beet selectedBeet;
    private Dictionary<Need, float> needsMet;

    [SerializeField]
    private List<BeetContainer> beetLocations;

    private void Awake()
    {
        settingsPanel.OnSettingsChanged += SettingsChanged;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(gameModelKey))
        {
            var data = PlayerPrefs.GetString(gameModelKey);
            var newData = Convert.FromBase64String(data);
            var newStream = new MemoryStream(newData);
            JsonReader reader = new JsonReader(newStream);
            reader.ReadObject(this);
        }

        ScreenNavigator.Instance.AddInputConsumer(this);
        needsMet = new Dictionary<Need, float>();
        nurseryGridRoot.GetAllAttached<TouchSensor>().ForEach(t => t.OnUpAsButton += NurseryGridTouched);
        releaseGrid.GetComponent<TouchSensor>().OnUpAsButton += ReleaseGridTouched;
        catchGrid.GetComponent<TouchSensor>().OnUpAsButton += CatchGridTouched;
        labGrid.GetComponent<TouchSensor>().OnUpAsButton += ToLabGridTouched;

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
            else
            {
                selectedBeet.MarkUnselected();
                selectedBeet = container.Beet;
                selectedBeet.MarkSelected();
            }
        }
    }
    
    private void ReleaseGridTouched(GameObject grid)
    {
        var container = grid.GetComponent<BeetContainer>();
        if (container == null || selectedBeet == null) return;
        if (selectedBeet.IsHealed == false) return;
        if (container.IsEmpty == false) return;

        selectedBeet.RemoveFromContainer();
        container.SetBeet(selectedBeet);
        selectedBeet.MarkUnselected();
        selectedBeet = null;
        Invoke("ReleaseBeet", 1f);
    }

    private void CatchGridTouched(GameObject grid)
    {
        var container = grid.GetComponent<BeetContainer>();
        if (container == null) return;
        if (container.IsEmpty == true) return;

        if (selectedBeet != null)
        {
            if (selectedBeet == container.Beet)
            {
                selectedBeet.MarkUnselected();
                selectedBeet = null;
                return;
            }
            else
            {
                selectedBeet.MarkUnselected();
            }
        }
        selectedBeet = container.Beet;
        selectedBeet.MarkSelected();
    }

    private void ReleaseBeet()
    {
        var container = releaseGrid.GetComponent<BeetContainer>();
        if (container.IsEmpty) return;

        var beet = container.RemoveBeet();
        Destroy(beet.gameObject);
    }

    private void ToLabGridTouched(GameObject grid)
    {
        var container = grid.GetComponent<BeetContainer>();
        if (container == null) return;
        if (container.IsEmpty == false) return;
        if (selectedBeet == null) return;

        var open = labGridRoot.GetAllAttached<BeetContainer>().FirstOrDefault(p => p.IsEmpty);
        if (open == null) return;

        selectedBeet.RemoveFromContainer();
        selectedBeet.MarkUnselected();
        container.SetBeet(selectedBeet);
        selectedBeet = null;

        Invoke("MoveToLab", 1f);
    }

    private void MoveToLab()
    {
        var open = labGridRoot.GetAllAttached<BeetContainer>().FirstOrDefault(p => p.IsEmpty);
        if (open == null) return;

        var beet = labGrid.GetComponent<BeetContainer>().Beet;
        beet.RemoveFromContainer();
        open.SetBeet(beet);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveTime();
    }

    public void OnApplicationQuit()
    {
        MemoryStream stream = new MemoryStream();
        JsonWriter writer = new JsonWriter(stream);
        writer.WriteObject(this);
        string data = Convert.ToBase64String(stream.ToArray());
        PlayerPrefs.SetString(gameModelKey, data);

        SaveTime();
    }

    private IEnumerator MainCoroutine()
    {
        LoadTimeAndUpdate();

        while (true)
        {
            yield return null;

            int delta = Mathf.RoundToInt(Time.deltaTime * 1000);
            UpdateWorld(delta);
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

    public bool IsActive()
    {
        return selectedBeet != null;
    }

    public void BeforeSerializing()
    {
        beetLocations = new List<BeetContainer>();
        foreach (var container in nurseryGridRoot.GetAllAttached<BeetContainer>())
            if (container.IsEmpty == false)
                beetLocations.Add(container);
    }

    public void AfterDeserializing()
    {
        foreach (var container in this.beetLocations)
        {
            var beet = Instantiate(beetPrefab.gameObject).GetComponent<Beet>();
            container.SetBeet(beet);
        }
    }

    public JsonModel[] GetChildren()
    {
        List<JsonModel> children = new List<JsonModel>();
        foreach (var container in nurseryGridRoot.GetAllAttached<BeetContainer>())
            if (container.IsEmpty == false)
                children.Add(container.Beet);
        return children.ToArray();
    }
}
