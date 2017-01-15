using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class InformationBarMediator : Mediator
{
    [Inject]
    public InformationBarView view { get; set; }

    [Inject]
    public FundsChangedSignal fundsChangedSignal { get; set; }

    [Inject]
    public ResearchProgressChangedSignal researchProgressChangedSignal { get; set; }

    public override void OnRegister()
    {
        fundsChangedSignal.AddListener(OnFundsChanged);
        researchProgressChangedSignal.AddListener(OnResearchProgressChanged);
    }

    private void OnFundsChanged(int newFunds)
    {
        view.SetFunds(newFunds);
    }

    private void OnResearchProgressChanged(float newProgess)
    {
        view.SetResearch(newProgess);
    }
}
