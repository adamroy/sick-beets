using UnityEngine;
using System.Collections.Generic;

public class SequencerData
{
    private List<Base> healthySequence = new List<Base>();
    public List<Base> HealthySequence { get { return healthySequence; } }

    private List<Base> unhealthySequence = new List<Base>();
    public List<Base> UnhealthySequence { get { return unhealthySequence; } }
}
