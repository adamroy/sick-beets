using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public interface IInputEnabler
{
    InputLayer InputLayer { get; }
    void SetInputEnabled(bool enabled);
}
