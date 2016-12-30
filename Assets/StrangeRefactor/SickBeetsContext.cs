using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

/// Overview!
/// Make your Mediator as thin as possible. Its function is to mediate
/// between view and app. Don't load it up with behavior that belongs in
/// the View (listening to/controlling interface), Commands (business logic),
/// Models (maintaining state) or Services (reaching out for data).

public class SickBeetsContext : MVCSContext
{
    public SickBeetsContext(MonoBehaviour view) : base(view) { }

    public SickBeetsContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }

    // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }

    // Override Start so that we can fire the StartSignal 
    override public IContext Start()
    {
        base.Start();
        StartSignal startSignal = injectionBinder.GetInstance<StartSignal>();
        startSignal.Dispatch();
        return this;
    }

    protected override void mapBindings()
    {
        // Injection bindings
        injectionBinder.Bind<IBeetPrefabLibrary>().ToValue(GameObject.FindObjectOfType<PrefabLibrary>());
        injectionBinder.Bind<ISickBeetsModel>().To<SickBeetsModel>().ToSingleton();
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
    }
}
