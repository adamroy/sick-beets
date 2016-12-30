using UnityEngine;
using System.Collections.Generic;
using System;

// Responsible for positioning and parenting a beet
public class BeetContainer : MonoBehaviour
{
    // Place to put the beet at
    public Transform targetTransform;

    public Beet Beet { get { return model.beetModel.GetComponent<Beet>(); } }
    public bool IsEmpty { get { return model == null || model.beetModel == null || Beet == null; } }

    private OldBeetContainerModel model;

    private void Awake()
    {
        model = GetComponent<OldBeetContainerModel>();
    }

    private void Start()
    {
        if(model.beetModel != null)
        {
            SetBeet(model.beetModel.GetComponent<Beet>());
        }
    }
    
    public void SetBeet(Beet inBeet)
    {
        if (model.beetModel != null && model.beetModel != inBeet.GetComponent<OldBeetModel>())
        {
            Debug.LogError("We've got a beet here already!");
            return;
        }

        model.beetModel = inBeet.GetComponent<OldBeetModel>();
        inBeet.transform.SetParent(this.transform, false);
        if (targetTransform != null)
        {
            inBeet.transform.position = targetTransform.position;
            inBeet.transform.rotation = targetTransform.rotation;
        }
        else
        {
            inBeet.transform.localPosition = Vector3.zero;
            inBeet.transform.localRotation = Quaternion.identity;
        }
    }

    public Beet RemoveBeet()
    {
        model.beetModel.transform.SetParent(null, true);
        var temp = model.beetModel;
        model.beetModel = null;
        return temp.GetComponent<Beet>();
    }
}
