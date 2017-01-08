using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class UIButtonPressedCommand : Command
{
    [Inject]
    public Button buttonPressed { get; set; }

    [Inject]
    public GameModel model { get; set; }

    [Inject]
    public ToggleEnvironmentSettingsPanelSignal toggleEnvironmentSettingsPanelSignal { get; set; }

    public override void Execute()
    {
        if (buttonPressed == Button.Menu)
        {
            if (model.GetCameraDestination() == CameraDestination.Nursery)
            {
                toggleEnvironmentSettingsPanelSignal.Dispatch();
            }
        }
        else if(buttonPressed == Button.Back)
        {

        }
    }
}
