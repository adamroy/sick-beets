using UnityEngine;
using System.Collections.Generic;
using System;


public class BeetContainerModel : MonoBehaviour, IJsonModelNode
{
    [HideInInspector]
    public BeetModel beetModel;

    [HideInInspector]
    [SerializeField]
    private GameObject beetPrefab;

    public void BeforeSerializing()
    {
        beetPrefab = beetModel != null ? beetModel.prefab : null;
    }

    public void AfterDeserializing()
    {
        var go = Instantiate(beetPrefab);
        go.transform.SetParent(this.transform);
        // The beet view will check for this later and attach it properly
        beetModel = go.GetComponent<BeetModel>();
    }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        var children = new List<IJsonModelNode>();
        children.Add(beetModel);
        return children;
    }
}