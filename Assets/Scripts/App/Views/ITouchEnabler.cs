using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public interface ITouchEnabler
{
    InputLayer InputLayer { get; }
    void SetTouchEnabled(bool enabled);
}
