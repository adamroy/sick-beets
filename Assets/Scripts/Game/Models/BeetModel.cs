using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class BeetModel
{
    private int instanceID;
    public int InstanceID { get { return instanceID; } set { instanceID = value; } }

    [SerializeField]
    private BeetType type;
    public BeetType Type { get { return type; } set { type = value; } }

    [SerializeField]
    private float health;
    public float Health { get { return health; } set { health = value; } }

    [SerializeField]
    private float lifeSpan;
    public float LifeSpan { get { return lifeSpan; } set { lifeSpan = value; } }

    [SerializeField]
    private List<string> environmentNeeds;
    [SerializeField]
    private List<float> environmentNeedValues;

    public BeetModel()
    {
        environmentNeeds = new List<string>();
        environmentNeedValues = new List<float>();
    }

    public void AddEnvironmentNeed(EnvironmentVariable variable, float value)
    {
        if (HasEnvironmentNeed(variable))
            throw new Exception("Beet model already containes need: " + variable.name);

        environmentNeeds.Add(variable.name);
        environmentNeedValues.Add(value);
    }

    public bool HasEnvironmentNeed(EnvironmentVariable need)
    {
        return environmentNeeds.Contains(need.name);
    }

    public float GetEnvironmentNeedValue(EnvironmentVariable need)
    {
        int index = environmentNeeds.IndexOf(need.name);
        return environmentNeedValues[index];
    }
}

public enum BeetType
{
    Common
}