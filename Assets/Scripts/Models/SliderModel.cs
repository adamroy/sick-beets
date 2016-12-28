using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SliderModel : MonoBehaviour, IJsonModelNode
{
    [Range(0f, 1f)]
    public float value;

    public void AfterDeserializing() { }

    public void BeforeSerializing() { }

    public IEnumerable<IJsonModelNode> GetChildren() { return null; }
}
