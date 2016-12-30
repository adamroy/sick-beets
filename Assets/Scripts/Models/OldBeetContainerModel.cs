using UnityEngine;
using System.Collections.Generic;
using System;


public class OldBeetContainerModel : MonoBehaviour, IJsonModelNode
{
    [HideInInspector]
    public OldBeetModel beetModel;

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
            beetModel = go.GetComponent<OldBeetModel>();
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