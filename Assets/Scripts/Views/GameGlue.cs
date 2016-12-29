using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(GameModel))]
public class GameGlue : MonoBehaviour, ScreenNavigator.InputConsumer
{
    
    
    private Beet selectedBeet;
    private Dictionary<Need, float> needsMet;
    private GameModel model;

    
    private void Awake()
    {
        model = GetComponent<GameModel>();
        needsMet = new Dictionary<Need, float>();
        model.settingsPanel.OnSettingsChanged += SettingsChanged;
    }

    private void Start()
    {
        ScreenNavigator.Instance.AddInputConsumer(this);
        model.nurseryGridRoot.GetAllAttached<TouchSensor>().ForEach(t => t.OnUpAsButton += NurseryGridTouched);
        model.releaseGrid.GetComponent<TouchSensor>().OnUpAsButton += ReleaseGridTouched;
        model.catchGrid.GetComponent<TouchSensor>().OnUpAsButton += CatchGridTouched;
        model.labGrid.GetComponent<TouchSensor>().OnUpAsButton += ToLabGridTouched;

        StartCoroutine(MainCoroutine());
    }

    private void SettingsChanged(Need need = null, float value = 0.5f)
    {
        if (need != null) needsMet[need] = value;

        foreach(var container in model.nurseryGridRoot.GetAllAttached<BeetContainer>())
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
        Invoke("ReleaseBeet", 3f);
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
        var container = model.releaseGrid.GetComponent<BeetContainer>();
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

        var open = model.labGridRoot.GetAllAttached<BeetContainer>().FirstOrDefault(p => p.IsEmpty);
        if (open == null) return;

        selectedBeet.RemoveFromContainer();
        selectedBeet.MarkUnselected();
        container.SetBeet(selectedBeet);
        selectedBeet = null;

        Invoke("MoveToLab", 3f);
    }

    private void MoveToLab()
    {
        var open = model.labGridRoot.GetAllAttached<BeetContainer>().FirstOrDefault(p => p.IsEmpty);
        if (open == null) return;
        var container = model.labGrid.GetComponent<BeetContainer>();
        if (container.IsEmpty) return;

        var beet = container.Beet;
        if (beet != null)
        {
            beet.RemoveFromContainer();
            open.SetBeet(beet);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            CancelInvoke("MoveToLab");
            MoveToLab();
            CancelInvoke("ReleaseBeet");
            ReleaseBeet();
            model.SaveGame();
        }
    }

    public void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    private IEnumerator MainCoroutine()
    {
        model.LoadGame();
        UpdateWorld(model.ElapsedMillisSinceLoad);

        while (true)
        {
            int delta = Mathf.RoundToInt(Time.deltaTime * 1000);
            UpdateWorld(delta);

            yield return null;
        }
    }

    private void UpdateWorld(float deltaTime)
    {
        EventManager.Broadcast(EventManager.Event.UpdateSimulation, deltaTime);
    }

    public bool IsActive()
    {
        return selectedBeet != null;
    }
}
