﻿using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CameraPositonChangedCommand : Command
{
    [Inject]
    public CameraDestination destination { get; set; }

    [Inject]
    public GameModel model { get; set; }

    public override void Execute()
    {
        model.SetCameraDestination(destination);
    }
}