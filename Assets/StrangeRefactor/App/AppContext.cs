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
        injectionBinder.Bind<GameModel>().ToSingleton().CrossContext();
        // This will be the signal child contexts listen to to begin
        injectionBinder.Bind<StartSignal>().ToSingleton().CrossContext();

        commandBinder.Bind<StartAppSignal>().To<LoadAppCommand>();
        commandBinder.Bind<LoadSceneSignal>().To<LoadSceneCommand>();
    }

    public override void Launch()
    {
        StartAppSignal startSignal = injectionBinder.GetInstance<StartAppSignal>();
        startSignal.Dispatch();
    }
}