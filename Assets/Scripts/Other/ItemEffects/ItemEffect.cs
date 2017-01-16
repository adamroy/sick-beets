using UnityEngine;
using System.Collections;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void Apply(RuntimeCofiguration runtimeConfiguration);
}
