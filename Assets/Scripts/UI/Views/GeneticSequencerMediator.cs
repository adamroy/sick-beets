using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class GeneticSequencerMediator : Mediator
{
    [Inject]
    public GeneticSequencerView view { get; set; }
    
    [Inject]
    public ToggleGeneticSequencerSignal toggleGeneticSequencerSignal { get; set; }

    [Inject]
    public CancelResearchBeetSignal cancelResearchBeetSignal { get; set; }
   
    [Inject]
    public ChangeActiveInputLayerSignal changeActiveInputLayerSignal { get; set; }

    [Inject]
    public SequenceResearchConfirmedSignal sequenceResearchConfirmedSignal { get; set; }

    public override void OnRegister()
    {
        view.Init();
        view.OnValidSequenceConfirmed.AddListener(ValidSequenceConfirmed);
        view.OnCancel.AddListener(Cancel);
        toggleGeneticSequencerSignal.AddListener(TogglePanel);
    }

    private void TogglePanel(SequencerData data)
    {
        if (data != null)
        {
            view.Display(data);
        }
        else
        {
            view.Hide();
        }
    }

    private void ValidSequenceConfirmed(List<Base> sequence)
    {
        view.Hide();
        changeActiveInputLayerSignal.Dispatch(InputLayer.Game, true);
        sequenceResearchConfirmedSignal.Dispatch(sequence);
    }

    private void Cancel()
    {
        view.Hide();
        changeActiveInputLayerSignal.Dispatch(InputLayer.Game, true);
        cancelResearchBeetSignal.Dispatch();
    }
}
