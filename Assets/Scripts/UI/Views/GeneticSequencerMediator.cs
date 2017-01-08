using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class GeneticSequencerMediator : Mediator
{
    [Inject]
    public GeneticSequencerView view { get; set; }
    
    [Inject]
    public ToggleGeneticSequencerSignal toggleGeneticSequencerSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        view.OnValidSequenceConfirmed.AddListener(ValidSequenceConfirmed);
        view.OnCancel.AddListener(Cancel);
        toggleGeneticSequencerSignal.AddListener(TogglePanel);
    }

    private void TogglePanel(SequencerData data)
    {
        view.Toggle(data);
    }

    private void ValidSequenceConfirmed(List<Base> sequence)
    {

    }

    private void Cancel()
    {

    }
}
