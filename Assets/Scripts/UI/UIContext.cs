using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;
using strange.extensions.signal.impl;

public class UIContext : MVCSSignalsContext
{
    public UIContext(MonoBehaviour view) : base(view) { }

    public UIContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }

    protected override void mapBindings()
    {
        mediationBinder.Bind<EnvironmentSettingsView>().To<EnvironmentSettingsMediator>();
        mediationBinder.Bind<TouchDetectorView>().To<TouchDetectorMediator>();

        commandBinder.Bind<StartSignal>();
        commandBinder.Bind<EnvironmentChangedSignal>().To<EnvironmentChangedCommand>();
    }
}