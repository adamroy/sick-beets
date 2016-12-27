using UnityEngine;
using System.Collections.Generic;

// Responsible for positioning and parenting a beet
public class BeetContainer : MonoBehaviour
{
    // Place to put the beet at
    public Transform targetTransform;

    public Beet Beet { get { return beet; } }
    public bool IsEmpty { get { return beet == null; } }

    private Beet beet;
    
    public void SetBeet(Beet inBeet)
    {
        if (beet != null)
        {
            Debug.LogError("We've got a beet here already!");
            return;
        }

        beet = inBeet;
        beet.transform.SetParent(this.transform, false);
        if (targetTransform != null)
        {
            beet.transform.position = targetTransform.position;
            beet.transform.rotation = targetTransform.rotation;
        }
        else
        {
            beet.transform.localPosition = Vector3.zero;
            beet.transform.localRotation = Quaternion.identity;
        }
    }

    public Beet RemoveBeet()
    {
        beet.transform.SetParent(null, true);
        var temp = beet;
        beet = null;
        return temp;
    }
}
