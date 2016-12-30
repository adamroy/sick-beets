using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BeetContainerModel
{
    [SerializeField]
    private int instanceID;
    public int InstanceID { get { return instanceID; } set { instanceID = value; } }

    [SerializeField]
    private BeetContainerFunction function;
    public BeetContainerFunction Function { get { return function; } set { function = value; } }
}

public enum BeetContainerFunction
{
    Nursery, 
    Input, 
    Output,
    LabTransfer,
    Lab
}