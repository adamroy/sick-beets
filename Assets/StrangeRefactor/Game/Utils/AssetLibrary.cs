using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using strange.extensions.injector.api;
using System;

public class AssetLibrary : View, IBeetPrefabLibrary, IEnvironmentVariableLibrary
{
    public BeetView commonBeetPrefab;
    public BeetView CommonBeetPrefab { get { return commonBeetPrefab; } }

    public List<EnvironmentVariable> environmentVariables;
    public IEnumerable<EnvironmentVariable> EnvironmentVariables { get { return environmentVariables; } }
}
