using UnityEngine;
using System.Collections;

public class BeetPot : GridItem
{
    private Beet beet;

    public void SetBeet(Beet inBeet)
    {
        if (beet != null)
            throw new System.Exception("We've got a beet here already!");

        beet = inBeet;
        beet.transform.SetParent(this.transform, false);
        beet.transform.localPosition = Vector3.zero;
        beet.transform.localRotation = Quaternion.identity;
    }

    public Beet RemoveBeet()
    {
        var temp = beet;
        beet.transform.SetParent(null, true);
        beet = null;
        return temp;
    }

    public override bool HandleTouch(GameObject obj)
    {
        if (beet != null && beet.HandleTouch())
        {
            // The beet has handled the touch
        }
        else
        {
            // This box can handle the touch
        }

        return true;
    }
}
