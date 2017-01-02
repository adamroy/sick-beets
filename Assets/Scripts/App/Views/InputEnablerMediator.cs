using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

// The base for mediators that deal with ITouchEnabler classes to allow them to talk to the app
public abstract class InputEnablerMediator : Mediator
{
    // Can't [Inject] interfaces (ITouchEnabler) so this inheritance based approach is a work around
    protected abstract IInputEnabler View { get; }

    // Apparently [Inject] tags are usable when inherited!
    [Inject]
    public SetInputLayerEnabledSignal setInputLayerEnabledSignal { get; set; }

    public override void OnRegister()
    {
        setInputLayerEnabledSignal.AddListener(EnableInputLayer);
    }

    private void EnableInputLayer(InputLayer layer, bool enabled)
    {
        if (View.InputLayer == layer)
        {
            View.SetInputEnabled(enabled);
        }
    }
}
