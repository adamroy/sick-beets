using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class ButtonInputMediator : Mediator
{
    [Inject]
    public ButtonInputView view { get; set; }

    [Inject]
    public ButtonPressedSignal buttonPressedSignal { get; set; }

    public override void OnRegister()
    {
        view.OnMenuPressed.AddListener(OnMenuPressed);
        view.OnBackPressed.AddListener(OnBackPressed);
    }

    private void OnMenuPressed()
    {
        buttonPressedSignal.Dispatch(Button.Menu);
    }

    private void OnBackPressed()
    {
        buttonPressedSignal.Dispatch(Button.Back);
    }
}
