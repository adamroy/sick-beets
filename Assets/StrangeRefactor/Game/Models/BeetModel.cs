using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class BeetModel
{
    [SerializeField]
    private BeetType type;
    public BeetType Type { get { return type; } set { type = value; } }

    [SerializeField]
    private int instanceID;
    public int InstanceID { get { return instanceID; } set { instanceID = value; } }

    [SerializeField]
    private float health;
    public float Health { get { return health; } set { health = value; } }

    [SerializeField]
    private float lifeSpan;
    public float LifeSpan { get { return lifeSpan; } set { lifeSpan = value; } }

    [SerializeField]
    private List<EnvironmentVariable> environmentNeeds;
    [SerializeField]
    private List<float> environmentNeedValues;

    public BeetModel()
    {
        environmentNeeds = new List<EnvironmentVariable>();
        environmentNeedValues = new List<float>();
    }

    public void AddEnvironmentNeed(EnvironmentVariable variable, float value)
    {
        if (HasEnvironmentNeed(variable))
            throw new Exception("Beet model already containes need: " + variable.name);

        environmentNeeds.Add(variable);
        environmentNeedValues.Add(value);
    }

    public IEnumerable<EnvironmentVariable> EnvironmentNeeds { get { return environmentNeeds; } }

    public bool HasEnvironmentNeed(EnvironmentVariable need)
    {
        return environmentNeeds.Contains(need);
    }

    public float GetEnvironmentNeedValue(EnvironmentVariable need)
    {
        int index = environmentNeeds.IndexOf(need);
        return environmentNeedValues[index];
    }
}

public enum BeetType
{
    Common
}