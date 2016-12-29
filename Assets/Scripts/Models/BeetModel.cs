using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class BeetModel : MonoBehaviour, IJsonModelNode
{
    public GameObject prefab;
    public int lifeSpan;
    public Color endColor;
    public Color selectedColor;
    public NeedRange[] needs;

    // Health ranges from 0 - 1
    // 0 = dead, 1 = cured
    [HideInInspector]
    public float health;

    // Ranges from -1 to +1
    [HideInInspector]
    public float healRate;
    
    [HideInInspector]
    public Color startColor;
    
    private void Awake()
    {
        health = Random.Range(0.1f, 0.5f);
    }

    public void BeforeSerializing() { }
    public void AfterDeserializing() { }
    public IEnumerable<IJsonModelNode> GetChildren() { return null; }
}

[Serializable]
public class NeedRange
{
    public Need Need;
    [Range(0f, 1f)]
    public float min, max;
    public float Value
    {
        get
        {
            if (value < 0)
                value = Random.Range(min, max);
            return value;
        }
    }
    private float value = -1;
}