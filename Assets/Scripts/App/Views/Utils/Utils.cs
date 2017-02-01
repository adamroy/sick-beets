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

    public static float CalculateBeatHealRate(BeetModel beet, AppModel model, IEnvironmentVariableLibrary environmentVariableLibrary)
    {
        // Calculate heal rate new each time to keep responsive
        float total = 0;
        int count = 0;
        foreach (var envNeed in environmentVariableLibrary.EnvironmentVariables)
        {
            if (beet.HasEnvironmentNeed(envNeed))
            {
                float needValue = beet.GetEnvironmentNeedValue(envNeed);
                float envValue = model.World.GetEnvironmentValue(envNeed);
                float diff = Mathf.Abs(needValue - envValue);
                float score = 1 - diff * 2;
                total += score;
                count++;
            }
        }

        float healRate = total / count;
        return healRate;
    }
}