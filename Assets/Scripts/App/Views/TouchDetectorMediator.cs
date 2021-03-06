﻿using UnityEngine;
using System.Collections;
using System;

public class TouchDetectorMediator : InputEnablerMediator
{
    [Inject]
    public TouchDetectorView view { get; set; }

    protected override IInputEnabler View { get { return view; } }
}
