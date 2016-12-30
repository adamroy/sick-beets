using UnityEngine;
using System.Collections;

public interface IBeetModel 
{
    /// <summary>
    /// The type of beet this model reps. Used for selecting prefabs to build when reinstantiating.
    /// </summary>
    BeetType Type { get; set; }

    /// <summary>
    /// The instance id of the view to connect this model to.
    /// Set when the beet is instantiated.
    /// </summary>
    int InstanceID { get; set; }

    /// <summary>
    /// Health between 0 and 1. 
    /// 0 is dead, 1 is healed.
    /// </summary>
    float Health { get; set; }

    /// <summary>
    /// The needs of this beet.
    /// </summary>
    NeedRange[] Needs { get; set; }
}

public enum BeetType
{
    Common
}