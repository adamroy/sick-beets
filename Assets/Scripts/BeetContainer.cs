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
            throw new System.Exception("We've got a beet here already!");

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

        SendMessage("BeetSet", beet, SendMessageOptions.DontRequireReceiver);
    }

    public Beet RemoveBeet()
    {
        var temp = beet;
        beet.transform.SetParent(null, true);
        beet = null;
        SendMessage("BeetRemoved", temp, SendMessageOptions.DontRequireReceiver);
        return temp;
    }
}
