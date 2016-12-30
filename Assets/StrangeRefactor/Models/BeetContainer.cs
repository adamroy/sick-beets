using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BeetContainerModel : IBeetContainerModel
{
    public int InstanceID { get; set; }
    public BeetContainerFunction function { get; set; }
}