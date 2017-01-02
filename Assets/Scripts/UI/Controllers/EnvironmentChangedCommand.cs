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
    public GameModel model { get; set; }

    public override void Execute()
    {
        model.SetEnvironmentValue(variable, value);
    }
}
