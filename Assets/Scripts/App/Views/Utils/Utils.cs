using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public static class Utils
{
    private static Dictionary<object, BeetView> beetMap = new Dictionary<object, BeetView>();
    private static Dictionary<object, BeetContainerView> containerMap = new Dictionary<object, BeetContainerView>();

    public static BeetView GetBeetView(Func<BeetView, bool> predicate)
    {
        return GameObject.FindObjectsOfType<BeetView>().First(predicate);
    }

    public static BeetView GetBeetViewByModel(BeetModel model)
    {
        if (!beetMap.ContainsKey(model))
            beetMap[model] = GetBeetView(b => b.GetInstanceID() == model.InstanceID);
        return beetMap[model];
    }

    public static BeetContainerView GetBeetContainerView(Func<BeetContainerView, bool> predicate)
    {
        return GameObject.FindObjectsOfType<BeetContainerView>().First(predicate);
    }

    public static BeetContainerView GetBeetContainerViewByModel(BeetContainerModel model)
    {
        if (!containerMap.ContainsKey(model))
            containerMap[model] = GetBeetContainerView(c => c.name == model.Name);
        return containerMap[model];
    }

    public static BeetContainerView GetBeetContainerViewByFunction(BeetContainerFunction function)
    {
        if (!containerMap.ContainsKey(function))
            containerMap[function] = GetBeetContainerView(c => c.function == function);
        return containerMap[function];
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