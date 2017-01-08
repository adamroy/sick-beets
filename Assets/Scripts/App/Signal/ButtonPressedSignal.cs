using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class ButtonPressedSignal : Signal<Button>
{

}

public enum Button
{
    Menu,
    Back
}
