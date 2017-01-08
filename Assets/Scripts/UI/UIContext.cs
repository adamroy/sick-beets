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
        injectionBinder.Bind<ToggleEnvironmentSettingsPanelSignal>().ToSingleton();

        mediationBinder.Bind<EnvironmentSettingsView>().To<EnvironmentSettingsMediator>();
        mediationBinder.Bind<TouchDetectorView>().To<TouchDetectorMediator>();

        commandBinder.Bind<StartSignal>().To<InitiateUICommand>();
        commandBinder.Bind<EnvironmentChangedSignal>().To<EnvironmentChangedCommand>();
        commandBinder.Bind<ButtonPressedSignal>().To<UIButtonPressedCommand>();
    }
}