using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class StoreItem : ScriptableObject
{
    public Sequence UnlockingSequence;

    [TextArea(2, 10)]
    public string description;
}
