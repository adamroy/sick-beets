using UnityEngine;
using System.Collections.Generic;
using strange.extensions.command.impl;

public class ResearchSequenceCommand : Command
{
    [Inject]
    public List<Base> sequence { get; set; }

    [Inject]
    public AppModel model { get; set; }

    public override void Execute()
    {
        model.Research.SetResearchSequence(sequence, 30f);
        model.Research.SetPhase(ResearchModel.Phase.Research);
    }
}
