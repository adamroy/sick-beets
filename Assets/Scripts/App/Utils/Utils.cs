using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public static class Utils
{
    public static BeetView GetBeetView(Func<BeetView, bool> predicate)
    {
        return GameObject.FindObjectsOfType<BeetView>().First(predicate);
    }

    public static BeetView GetBeetViewByModel(BeetModel model)
    {
        return GetBeetView(b => b.GetInstanceID() == model.InstanceID);
    }

    public static BeetContainerView GetBeetContainerView(Func<BeetContainerView, bool> predicate)
    {
        return GameObject.FindObjectsOfType<BeetContainerView>().First(predicate);
    }

    public static BeetContainerView GetBeetContainerViewByModel(BeetContainerModel model)
    {
        return GetBeetContainerView(c => c.name == model.Name);
    }

    public static BeetContainerView GetBeetContainerViewByFunction(BeetContainerFunction function)
    {
        return GetBeetContainerView(c => c.function == function);
    }
}