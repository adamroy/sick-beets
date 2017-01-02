using UnityEngine;
using System.Collections;
using System;

public class TouchDetectorMediator : TouchEnablerMediator
{
    [Inject]
    public TouchDetectorView view { get; set; }

    protected override ITouchEnabler View { get { return view; } }
}
