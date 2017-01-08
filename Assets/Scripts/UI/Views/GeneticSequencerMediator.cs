using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class GeneticSequencerMediator : Mediator
{
    [Inject]
    public GeneticSequencerView view { get; set; }
    
    [Inject]
    public ToggleGeneticSequencerSignal toggleGeneticSequencerSignal { get; set; }

    public override void OnRegister()
    {
        toggleGeneticSequencerSignal.AddListener(TogglePanel);
    }

    private void TogglePanel(SequencerData data)
    {
        view.Toggle(data);
    }
}
