using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BeetContainerModel
{
    // The name of the gameobject in the scene distinguishes the view. Make it unique!
    [SerializeField]
    private string name;
    public string Name { get { return name; } set { name = value; } }

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