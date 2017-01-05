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

public class GameContext : MVCSSignalsContext
{
    public GameContext(MonoBehaviour view) : base(view) { }

    public GameContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }
    
    protected override void mapBindings()
    {
        // Injection bindings
        injectionBinder.Bind<BeetCreatedSignal>().ToSingleton();
        injectionBinder.Bind<SelectBeetSignal>().ToSingleton();
        injectionBinder.Bind<PlaceBeetSignal>().ToSingleton();
        injectionBinder.Bind<BeetModelUpdatedSignal>().ToSingleton();
        injectionBinder.Bind<PlaceCameraSignal>().ToSingleton();
        
        // Mediation bindings
        mediationBinder.Bind<BeetContainerView>().To<BeetContainerMediator>();
        mediationBinder.Bind<BeetView>().To<BeetMediator>();
        mediationBinder.Bind<TouchDetectorView>().To<TouchDetectorMediator>();
        mediationBinder.Bind<CameraPannerView>().To<CameraPannerMediator>();

        // Command bindings
        commandBinder.Bind<StartSignal>()
            .To<InstantiateModelCommand>()
            .To<UpdateModelRoutineCommand>()
            .Once().InSequence();
        commandBinder.Bind<ContainerSelectedSignal>()
            .To<ContainerSelectedCommand>();
        commandBinder.Bind<DestroyBeetSignal>()
            .To<DestroyBeetCommand>();
        commandBinder.Bind<RequestBeetCreationSignal>()
            .To<CreateBeetCommand>();
        commandBinder.Bind<ResearchBeetSignal>()
            .To<TransferToLabCommand>()
            .To<PanToLabCommand>()
            .InSequence();
        commandBinder.Bind<CameraPositionChangedSignal>()
            .To<CameraPositonChangedCommand>();
    }
}
