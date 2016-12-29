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
        if (beetPrefab != null)
        {
            var go = Instantiate(beetPrefab);
            go.transform.SetParent(this.transform);
            // The beet container view will check for this later and attach it properly
            beetModel = go.GetComponent<BeetModel>();
        }
    }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        if (beetModel == null)
            return null;

        var children = new List<IJsonModelNode>();
        children.Add(beetModel);
        return children;
    }
}