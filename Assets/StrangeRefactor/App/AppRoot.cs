using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

public class AppRoot : ContextView
{
    private void Awake()
    {
        context = new AppContext(this);
    }
}
