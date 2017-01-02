using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using strange.extensions.command.impl;
using strange.extensions.context.api;

public class LoadSceneCommand : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    [Inject]
    public string sceneName { get; set; }

    [Inject]
    public LoadSceneMode loadSceneMode { get; set; }

    [Inject]
    public Action<string> sceneLoadedCallback { get; set; }

    public override void Execute()
    {
        Retain();
        contextView.GetComponent<MonoBehaviour>().StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        var asyncOp = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        yield return asyncOp;
        sceneLoadedCallback(sceneName);
        yield return null; // Wait an extra frame to hopefully hit the Start() phase
        Release();
    }
}
