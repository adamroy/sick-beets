using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.injector.api;
using System;

public class PrefabLibrary : View, IBeetPrefabLibrary
{
    public BeetView commonBeetPrefab;
    public BeetView CommonBeetPrefab { get { return commonBeetPrefab; } }
}
