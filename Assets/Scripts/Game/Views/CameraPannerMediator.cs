using UnityEngine;
using System.Collections;
using System;

public class CameraPannerMediator : InputEnablerMediator
{
    [Inject]
    public CameraPannerView view { get; set; }

    protected override IInputEnabler View { get { return view; } }
}
