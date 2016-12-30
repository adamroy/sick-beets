using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

/// Overview!
/// Make your Mediator as thin as possible. Its function is to mediate
/// between view and app. Don't load it up with behavior that belongs in
/// the View (listening to/controlling interface), Commands (business logic),
/// Models (maintaining state) or Services (reaching out for data).

public class SickBeetsContext : MVCSSignalsContext
{
    public SickBeetsContext(MonoBehaviour view) : base(view) { }

    public SickBeetsContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }
    
    protected override void mapBindings()
    {
        // Injection bindings
        injectionBinder.Bind<IBeetPrefabLibrary>().ToValue(GameObject.FindObjectOfType<PrefabLibrary>());
        injectionBinder.Bind<SickBeetsModel>().ToSingleton();
        injectionBinder.Bind<BeetCreatedSignal>().ToSingleton();
        injectionBinder.Bind<SelectBeetSignal>().ToSingleton();
        injectionBinder.Bind<PlaceBeetSignal>().ToSingleton();
        
        // Mediation bindings
        mediationBinder.Bind<BeetContainerView>().To<BeetContainerMediator>();
        mediationBinder.Bind<BeetView>().To<BeetMediator>();

        // Command bindings
        commandBinder.Bind<StartSignal>()
            .To<LoadModelCommand>()
            .To<InstantiateModelCommand>()
            .To<StartCommand>()
            .To<CreateBeetCommand>()
            .Once().InSequence();
        commandBinder.Bind<QuitSignal>()
            .To<SaveModelCommand>()
            .Once();
        commandBinder.Bind<ContainerSelectedSignal>()
            .To<ContainerSelectedCommand>();
        commandBinder.Bind<DestroyBeetSignal>()
            .To<DestroyBeetCommand>();
        commandBinder.Bind<RequestBeetCreationSignal>()
            .To<CreateBeetCommand>();
        commandBinder.Bind<TransferToLabSignal>()
            .To<TransferToLabCommand>();
        commandBinder.Bind<PauseSignal>()
            .To<SaveModelCommand>();
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
