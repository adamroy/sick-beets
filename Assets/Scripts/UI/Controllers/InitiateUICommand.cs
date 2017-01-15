using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

// Takes the saved model and sends out appropriate signals to update UI to match
public class InitiateUICommand : Command
{
    [Inject]
    public AppModel model { get; set; }

    [Inject]
    public EnvironmentChangedSignal setEnvironmentVariable { get; set; }

    [Inject]
    public IEnvironmentVariableLibrary environmentVariableLibrary { get; set; }

    [Inject]
    public FundsChangedSignal fundsChangedSignal { get; set; }

    [Inject]
    public ResearchProgressChangedSignal researchProgressChangedSignal { get; set; }

    public override void Execute()
    {
        foreach(var variable in environmentVariableLibrary.EnvironmentVariables)
        {
            setEnvironmentVariable.Dispatch(variable, model.World.GetEnvironmentValue(variable));
        }

        fundsChangedSignal.Dispatch(model.GetFunds());
        researchProgressChangedSignal.Dispatch(model.Research.Progress);
    }
}
