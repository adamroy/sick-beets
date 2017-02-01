using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;
using strange.extensions.context.api;

public class UpdateModelRoutineCommand : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    [Inject]
    public AppModel model { get; set; }

    [Inject]
    public IEnvironmentVariableLibrary environmentVariableLibrary { get; set; }

    [Inject]
    public BeetModelUpdatedSignal beetModelUpdateSignal { get; set; }

    [Inject]
    public ResearchCompleteSignal researchCompleteSignal { get; set; }

    [Inject]
    public ResearchProgressChangedSignal researchProgressChangedSignal { get; set; }

    private Coroutine updateCoroutine;

    public override void Execute()
    {
        Retain();
        updateCoroutine = contextView.GetComponent<MonoBehaviour>().StartCoroutine(UpdateCoroutine());
    }

    public override void Release()
    {
        contextView.GetComponent<MonoBehaviour>().StopCoroutine(updateCoroutine);
        base.Release();
    }

    private IEnumerator UpdateCoroutine()
    {
        float deltaTime = Convert.ToSingle(CurrentSeconds() - model.GetTime());
        model.SetTime(CurrentSeconds());
        UpdateModel(deltaTime);
        // Use realtime since startup so that pauses don't interupt updates
        float previousFrame = Time.realtimeSinceStartup;

        yield return null;

        while (true)
        {
            // Save time constantly so we're good whenever we quit
            model.SetTime(CurrentSeconds());
            float delta = Time.realtimeSinceStartup - previousFrame;
            UpdateModel(delta);
            previousFrame = Time.realtimeSinceStartup;
            yield return null;
        }
    }

    private void UpdateModel(float deltaTime)
    {
        var nurseryContainers = model.World.GetAllContainersByFunction(BeetContainerFunction.Nursery);
        foreach (var c in nurseryContainers)
        {
            var beet = model.World.GetBeetAssignment(c);
            if (beet != null)
                UpdateBeetHealth(beet, deltaTime);
        }

        UpdateResearch(deltaTime);
    }

    private void UpdateBeetHealth(BeetModel beet, float deltaTime)
    {
        float healRate = Utils.CalculateBeatHealRate(beet, model, environmentVariableLibrary);
        beet.Health += (deltaTime / beet.LifeSpan) * healRate;
        beet.Health = Mathf.Clamp01(beet.Health); // Health must be in range 0-1
        beetModelUpdateSignal.Dispatch(beet);
    }

    private void UpdateResearch(float deltaTime)
    {
        if (model.Research.GetPhase() == ResearchModel.Phase.Research)
        {
            model.Research.Progress = Mathf.Clamp01(model.Research.Progress + deltaTime / model.Research.TimeToResearch);
            researchProgressChangedSignal.Dispatch(model.Research.Progress);

            if (model.Research.Progress == 1f)
            {
                model.Research.SetPhase(ResearchModel.Phase.Results);
                researchCompleteSignal.Dispatch();
            }
        }
        else if (model.Research.GetPhase() == ResearchModel.Phase.Idle)
        {
            researchProgressChangedSignal.Dispatch(0f);
        }
    }

    // Seconds since UTC epoch
    public static long CurrentSeconds()
    {
        return ((DateTime.UtcNow.Ticks - 621355968000000000) / TimeSpan.TicksPerSecond);
    }
}