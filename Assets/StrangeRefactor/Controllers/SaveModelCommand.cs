﻿using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

// Save the model to playerprefs or somewhere
public class SaveModelCommand : Command
{
    public const string SaveGameKey = "GameSave";

    // Since this is called from the pause signal
    [Inject]
    public bool pause { get; set; }

    [Inject]
    public GameModel model { get; set; }
    
    public override void Execute()
    {
        if (pause)
        {
            JsonSavingUtility.Save(SaveGameKey, model, true);
        }
    }
}