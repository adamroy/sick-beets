﻿using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;
using strange.extensions.signal.impl;

public class AppContext : MVCSSignalsContext
{
    public AppContext(MonoBehaviour view) : base(view) { }

    public AppContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }

    protected override void mapBindings()
    {
        // Various data that can be set in the Editor by editing the gameobject
        injectionBinder
            .Bind<IBeetPrefabLibrary>()
            .Bind<IEnvironmentVariableLibrary>()
            .Bind<IBaseLibrary>()
            .Bind<ISequenceLibrary>()
            .ToValue(GameObject.FindObjectOfType<AssetLibrary>()).CrossContext();

        // The model used throughout the game
        injectionBinder.Bind<GameModel>().ToSingleton().CrossContext();
        // This will be the signal child contexts listen to to begin
        injectionBinder.Bind<StartSignal>().ToSingleton().CrossContext();
        // This lets any part of the app change the active input layer
        injectionBinder.Bind<ChangeActiveInputLayerSignal>().ToSingleton().CrossContext();
        injectionBinder.Bind<SetInputLayerEnabledSignal>().ToSingleton().CrossContext();
        // Researching a beet effects the Game and UI
        injectionBinder.Bind<ResearchBeetSignal>().ToSingleton().CrossContext();
        // Lets whole app listen to button presses
        injectionBinder.Bind<ButtonPressedSignal>().ToSingleton().CrossContext();

        mediationBinder.Bind<ButtonInputView>().To<ButtonInputMediator>();

        commandBinder.Bind<StartAppSignal>()
            .To<LoadModelCommand>()
            .To<LoadAppCommand>();
        commandBinder.Bind<PauseSignal>()
            .To<SaveModelCommand>();
        commandBinder.Bind<LoadSceneSignal>()
            .To<LoadSceneCommand>();
        commandBinder.Bind<ChangeActiveInputLayerSignal>()
            .To<ChangeActiveInputLayerCommand>();
    }

    public override void Launch()
    {
        var startSignal = injectionBinder.GetInstance<StartAppSignal>();
        startSignal.Dispatch();
    }

    public override void OnApplicationPause(bool pause)
    {
        var pauseSignal = injectionBinder.GetInstance<PauseSignal>();
        pauseSignal.Dispatch(pause);
    }

    public override void OnApplicationQuit()
    {
        var pauseSignal = injectionBinder.GetInstance<PauseSignal>();
        pauseSignal.Dispatch(true);
    }
}