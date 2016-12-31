using UnityEngine;
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
        // The model used throughout the game
        injectionBinder.Bind<GameModel>().ToSingleton().CrossContext();

        // This will be the signal child contexts listen to to begin
        injectionBinder.Bind<StartSignal>().ToSingleton().CrossContext();

        commandBinder.Bind<StartAppSignal>()
            .To<LoadModelCommand>()
            .To<LoadAppCommand>();
        commandBinder.Bind<PauseSignal>()
            .To<SaveModelCommand>();
        commandBinder.Bind<LoadSceneSignal>()
            .To<LoadSceneCommand>();
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