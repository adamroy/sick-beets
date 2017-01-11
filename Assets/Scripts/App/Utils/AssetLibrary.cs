using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using strange.extensions.injector.api;
using System;

public class AssetLibrary : View, IBeetPrefabLibrary, IEnvironmentVariableLibrary, IBaseLibrary, ISequenceLibrary, IStoreItemLibrary
{
    public BeetView commonBeetPrefab;
    public BeetView CommonBeetPrefab { get { return commonBeetPrefab; } }

    public List<EnvironmentVariable> environmentVariables;
    public IEnumerable<EnvironmentVariable> EnvironmentVariables { get { return environmentVariables; } }

    public List<Base> bases;
    public IEnumerable<Base> Bases { get { return bases; } }

    public List<Sequence> sequences;
    public IEnumerable<Sequence> Sequences { get { return sequences; } }

    public List<StoreItem> storeItems;
    public IEnumerable<StoreItem> StoreItems { get { return storeItems; } }
}
