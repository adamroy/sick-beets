using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ResearchModel
{
    public enum Phase
    {
        Idle,
        GeneSelection,
        Research,
        Results
    }

    [SerializeField]
    private Phase phase;

    [SerializeField]
    private List<Base> researchSequence;

    [SerializeField]
    private float timeToResearch;
    public float TimeToResearch { get { return timeToResearch; } }

    [SerializeField]
    private float progress;
    public float Progress { get { return progress; } set { progress = value; } }

    #region public data methods

    public ResearchModel()
    {
        Clear();
    }

    public void SetPhase(Phase p)
    {
        phase = p;
    }

    public Phase GetPhase()
    {
        return phase;
    }

    public void SetResearchSequence(List<Base> seq, float time)
    {
        researchSequence = seq;
        timeToResearch = time;
    }

    public List<Base> GetResearchSequence()
    {
        return researchSequence;
    }

    public float GetTimeToResearch()
    {
        return timeToResearch;
    }

    public void Clear()
    {
        phase = Phase.Idle;
        researchSequence = null;
        timeToResearch = 0f;
        progress = 0f;
    }

    #endregion
}
