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
        commandBinder.Bind<StartSignal>();
    }
}