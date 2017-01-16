using UnityEngine;
using System.Collections;
using System;

public class JukeBoxMediator : ItemMediator
{
    [Inject]
    public JukeBoxView view { get; set; }

    public override IItemView itemView { get { return view; } }


    public override void OnRegister()
    {
        base.OnRegister();
        view.Deactivate();
    }
}
