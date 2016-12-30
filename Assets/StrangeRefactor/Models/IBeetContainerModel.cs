using UnityEngine;
using System.Collections;
using System;

public interface IBeetContainerModel
{
    /// <summary>
    /// The instance id of the view to connect this model to.
    /// View should be static since it is part of the scene.
    /// </summary>
    int InstanceID { get; set; }

    /// <summary>
    /// What this container does. This helps keep container code to one class 
    /// (if code specific to one function grows too large, may change to class/subclasses)
    /// </summary>
    BeetContainerFunction function { get; set; }
}

public enum BeetContainerFunction
{
    Nursery, 
    Input,
    Output,
    LabTransfer,
    Lab
}