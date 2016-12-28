using UnityEngine;
using System.Collections.Generic;
using System;


public class BeetContainerModel : MonoBehaviour, IJsonModelNode
{
    public BeetModel beetModel;

    [HideInInspector]
    [SerializeField]
    private GameObject beetPrefab;

    public void BeforeSerializing()
    {
        beetPrefab = beetModel.prefab;
    }

    public void AfterDeserializing()
    {
        var go = Instantiate(beetPrefab);

    }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        var children = new List<IJsonModelNode>();
        children.Add(beetModel);
        return children;
    }
}