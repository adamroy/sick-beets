using UnityEngine;
using System.Collections.Generic;

public interface ISequenceLibrary
{
    IEnumerable<Sequence> Sequences { get; }
}
