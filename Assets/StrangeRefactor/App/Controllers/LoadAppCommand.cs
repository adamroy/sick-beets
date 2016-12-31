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

    private List<string> scenesToLoad = new List<string>() { "strange" };

    public override void Execute()
    {
        Retain();

        foreach (string sceneName in scenesToLoad)
            loadSceneSignal.Dispatch(sceneName, LoadSceneMode.Additive, OnSceneLoaded);
    }

    private void OnSceneLoaded(string sceneName)
    {
        scenesToLoad.Remove(sceneName);
        if (scenesToLoad.Count == 0)
        {
            startSignal.Dispatch();
            Release();
        }
    }
}
