using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class TouchDetector : View
{
	public Signal touchSignal = new Signal();
		
	void OnMouseUpAsButton()
	{
		touchSignal.Dispatch();
	}
}