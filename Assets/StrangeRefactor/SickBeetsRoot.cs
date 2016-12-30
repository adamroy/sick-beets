using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

public class SickBeetsRoot : ContextView
{
    private void Awake()
    {
        context = new SickBeetsContext(this);
    }
}
