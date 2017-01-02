using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

public class LoadAppCommand : Command
{
    [Inject]
    public LoadSceneSignal loadSceneSignal { get; set; }
    
    [Inject]
    public StartSignal startSignal { get; set; }
    
    public override void Execute()
    {
        Retain();

        List<string> scenesToLoad = new List<string>() { "game", "ui" };

        foreach (string sceneName in scenesToLoad)
            loadSceneSignal.Dispatch(sceneName, LoadSceneMode.Additive, 
            (sceneLoaded) =>
            {
                scenesToLoad.Remove(sceneLoaded);
                if (scenesToLoad.Count == 0)
                {
                    startSignal.Dispatch();
                    Release();
                }
            });
    }
}
