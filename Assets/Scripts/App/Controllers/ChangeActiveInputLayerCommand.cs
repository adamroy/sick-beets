using UnityEngine;
using System;
using System.Linq;
using strange.extensions.command.impl;

public class ChangeActiveInputLayerCommand : Command
{
    [Inject]
    public InputLayer layer { get; set; }

    [Inject]
    public bool enable { get; set; }

    [Inject]
    public SetInputLayerEnabledSignal setInputLayerEnabledSignal { get; set; }

    private static bool debug = false;

    public override void Execute()
    {

        if (debug)
            Debug.Log("Request:\n" + Enum.GetName(typeof(InputLayer), layer) + ": " + enable);

        foreach (var currentLayer in Enum.GetValues(typeof(InputLayer)).Cast<InputLayer>())
        {
            if (enable)
            {
                bool enableCurrentLayer = (int)currentLayer >= (int)layer;
                if (debug)
                    Debug.Log("Setting: " + Enum.GetName(typeof(InputLayer), currentLayer) + ": " + enableCurrentLayer);
                setInputLayerEnabledSignal.Dispatch(currentLayer, enableCurrentLayer);
            }
            else
            {
                bool enableCurrentLayer = true;
                if (debug)
                    Debug.Log("Setting: " + Enum.GetName(typeof(InputLayer), currentLayer) + ": " + enableCurrentLayer);
                setInputLayerEnabledSignal.Dispatch(currentLayer, enableCurrentLayer);
            }
        }
    }
}

public enum InputLayer
{
    Game = 0,
    Camera = 1,
    UI = 2
}