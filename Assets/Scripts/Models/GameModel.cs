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
        MemoryStream stream = new MemoryStream();
        JsonWriter writer = new JsonWriter(stream);
        writer.WriteObject(this);
        string data = Convert.ToBase64String(stream.ToArray());
        PlayerPrefs.SetString(gameModelKey, data);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(gameModelKey))
        {
            var data = PlayerPrefs.GetString(gameModelKey);
            var newData = Convert.FromBase64String(data);
            var newStream = new MemoryStream(newData);
            JsonReader reader = new JsonReader(newStream);
            reader.ReadObject(this);
        }
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
        children.AddRange(nurseryGridRoot.GetAllAttached<BeetContainerModel>().Cast<IJsonModelNode>());
        return children;
    }
}
