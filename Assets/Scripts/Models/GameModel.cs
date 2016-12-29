using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class GameModel : MonoBehaviour, IJsonModelNode
{
    private const string gameModelKey = "GameData";

    public float ElapsedMillisSinceLoad { get; private set; }

    public AutoGrid nurseryGridRoot;
    public AutoGrid labGridRoot;
    public AutoGrid releaseGrid;
    public AutoGrid catchGrid;
    public AutoGrid labGrid;
    public NurserySettingsPanel settingsPanel;
    public NurserySettingsPanelModel settingsPanelModel;
    public ScreenNavigator navigator;

    [HideInInspector]
    [SerializeField]
    private long time;

    [HideInInspector]
    [SerializeField]
    private List<SavedBeet> savedBeets;

    [Serializable]
    class SavedBeet
    {
        public BeetContainer container;
        public GameObject prefab;
    }

    public void  SaveGame()
    {
        JsonSavingUtility.Save(gameModelKey, this);
    }

    public void LoadGame()
    {
        JsonSavingUtility.Load(gameModelKey, this);
    }

    // Seconds since UTC epoch
    private long CurrentSeconds()
    {
        return ((DateTime.UtcNow.Ticks - 621355968000000000) / TimeSpan.TicksPerSecond);
    }

    public void BeforeSerializing()
    {
        time = CurrentSeconds();
    }

    public void AfterDeserializing()
    {
        var elapsedSeconds = CurrentSeconds() - time;
        ElapsedMillisSinceLoad = Convert.ToSingle(elapsedSeconds * 1000);
    }

    IEnumerable<IJsonModelNode> IJsonModelNode.GetChildren()
    {
        List<IJsonModelNode> children = new List<IJsonModelNode>();
        children.Add(settingsPanelModel);
        children.Add(navigator);
        children.Add(catchGrid.GetComponent<IJsonModelNode>());
        children.AddRange(nurseryGridRoot.GetAllAttached<BeetContainerModel>().Cast<IJsonModelNode>());
        children.AddRange(labGridRoot.GetAllAttached<BeetContainerModel>().Cast<IJsonModelNode>());
        return children;
    }
}
