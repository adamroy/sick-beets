using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class AppModel : IJsonModelNode
{
    // Fields that don't need to be saved but are passed around with the model
    public bool SuccessfulyLoaded { get; set; }

    [SerializeField]
    private long time;

    [SerializeField]
    private int funds;

    private WorldModel world;
    public WorldModel World { get { return world; } }
    
    [SerializeField]
    private ResearchModel research;
    public ResearchModel Research { get { return research; } }

    [SerializeField]
    private StoreModel store;
    public StoreModel Store { get { return store; } }

    public AppModel()
    {
        SuccessfulyLoaded = false;
        time = 0;
        funds = 0;
        world = new WorldModel();
        research = new ResearchModel();
        store = new StoreModel();
    }

    public void Clear()
    {
        SuccessfulyLoaded = false;
        time = 0;
        funds = 0;
        World.Clear();
        Research.Clear();
        Store.Clear();
    }

    #region public data methods

    public void SetTime(long time)
    {
        this.time = time;
    }

    public long GetTime()
    {
        return this.time;
    }

    public int GetFunds()
    {
        return funds;
    }

    public bool MakeTransaction(int amount)
    {
        if (funds + amount < 0)
            return false;

        funds += amount;
        return true;
    }

    #endregion

    #region IJsonModelNode

    public void BeforeSerializing() { }

    public void AfterDeserializing() { }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        List<IJsonModelNode> children = new List<IJsonModelNode>();
        children.Add(world);
        return children;
    }

    #endregion
}