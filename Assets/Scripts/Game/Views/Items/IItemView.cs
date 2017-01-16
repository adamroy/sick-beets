using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public interface IItemView
{
    StoreItem Item { get; }
    void Activate();
}
