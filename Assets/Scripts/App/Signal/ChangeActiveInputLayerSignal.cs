using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

// This signal is to change the app's active input layer (a request made from somewhere in the app)
public class ChangeActiveInputLayerSignal : Signal<InputLayer, bool>
{

}