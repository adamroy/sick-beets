using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

// This signal is to tell objects on the specified input layer to enable/disable based upon the boolean value
public class SetInputLayerEnabledSignal : Signal<InputLayer, bool>
{

}
