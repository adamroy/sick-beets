using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class DestroyBeetCommand : Command
{
    // The beet to destroy
    [Inject]
    public BeetView beetView { get; set; }

    // The container to remove it from
    [Inject]
    public BeetContainerView containerView { get; set; }

    // How many seconds to wait before obliterating the fella
    [Inject]
    public float delay { get; set; }

    [Inject]
    public GameModel model { get; set; }

    public override void Execute()
    {
        model.RemoveBeet(model.GetBeetByID(beetView.GetInstanceID()));
        GameObject.Destroy(beetView.gameObject, delay);
    }
}
