using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

public class UIRoot : ContextView
{
    private void Awake()
    {
        context = new UIContext(this);
    }
}