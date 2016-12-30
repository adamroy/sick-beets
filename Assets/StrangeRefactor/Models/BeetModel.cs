using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BeetModel : IBeetModel
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
    private NeedRange[] needs;
    public NeedRange[] Needs { get { return needs; } set { needs = value; } }
}
