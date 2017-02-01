using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class EnvironmentChangedCommand : Command
{
    [Inject]
    public EnvironmentVariable variable { get; set; }

    [Inject]
    public float value { get; set; }

    [Inject]
    public AppModel model { get; set; }

    [Inject]
    public IEnvironmentVariableLibrary environmentVariableLibrary { get; set; }

    [Inject]
    public DisplayHealRateSignal displayHealRateSignal { get; set; }

    public override void Execute()
    {
        model.World.SetEnvironmentValue(variable, value);

        var nurseryContainers = model.World.GetAllContainersByFunction(BeetContainerFunction.Nursery);
        foreach (var c in nurseryContainers)
        {
            var beet = model.World.GetBeetAssignment(c);
            if (beet != null)
            {
                float healRate = Utils.CalculateBeatHealRate(beet, model, environmentVariableLibrary);
                var containerView = Utils.GetBeetContainerViewByModel(c);
                displayHealRateSignal.Dispatch(containerView, healRate);
            }
        }
    }
}
