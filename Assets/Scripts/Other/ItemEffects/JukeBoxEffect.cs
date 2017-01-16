using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Effects/Jukebox")]
public class JukeBoxEffect : ItemEffect
{
    public override void Apply(RuntimeCofiguration runtimeConfiguration)
    {
        Debug.Log("Applying jukebox effect");
    }
}
