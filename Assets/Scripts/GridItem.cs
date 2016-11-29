using UnityEngine;
using System.Collections;

public class GridItem : MonoBehaviour
{
    public virtual bool HandleTouch(GameObject obj)
    {
        return false;
    }
}
