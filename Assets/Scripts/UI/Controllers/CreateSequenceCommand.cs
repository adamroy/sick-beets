using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CreateSequenceCommand : Command
{
    // The beet we are creating the sequence for
    [Inject]
    public BeetModel beetModel { get; set; }

    // Give the panel the information then raise it
    [Inject]
    public ToggleGeneticSequencerSignal toggleGeneticSequencerSignal { get; set; }

    [Inject]
    public IBaseLibrary baseLibrary { get; set; }

    [Inject]
    public ISequenceLibrary sequenceLibrary { get; set; }

    public override void Execute()
    {
        var sequencerData = new SequencerData();
        sequencerData.HealthySequence.AddRange(baseLibrary.Bases);
        sequencerData.HealthySequence.AddRange(baseLibrary.Bases);
        sequencerData.UnhealthySequence.AddRange(baseLibrary.Bases);
        sequencerData.UnhealthySequence.AddRange(baseLibrary.Bases);
        toggleGeneticSequencerSignal.Dispatch(sequencerData);
    }
}
