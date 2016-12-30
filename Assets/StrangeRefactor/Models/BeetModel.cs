using UnityEngine;
using System.Collections;
using System;

public class BeetModel : IBeetModel
{
    public BeetType Type { get; set; }
    public int InstanceID { get; set; }
    public float Health { get; set; }
    public NeedRange[] Needs { get; set; }
}
