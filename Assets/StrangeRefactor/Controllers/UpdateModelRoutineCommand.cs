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
    public GameModel model { get; set; }

    private Coroutine updateCoroutine;

    public override void Execute()
    {
        Retain();
        updateCoroutine = contextView.GetComponent<MonoBehaviour>().StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        float deltaTime = Convert.ToSingle(CurrentSeconds() - model.Time);
        model.Time = CurrentSeconds();
        UpdateModel(deltaTime);

        yield return null;

        while (true)
        {
            // Save time constantly so we're good whenever we quit
            model.Time = CurrentSeconds();
            UpdateModel(Time.deltaTime);
            yield return null;
        }
    }

    private void UpdateModel(float deltaTime)
    {
        var nurseryContainers = model.GetContainersByFunction(BeetContainerFunction.Nursery);
        foreach (var c in nurseryContainers)
        {
            var beet = model.GetBeetAssignment(c);
            if (beet != null)
                UpdateBeetHealth(beet, deltaTime);
        }
    }

    private void UpdateBeetHealth(BeetModel beet, float deltaTime)
    {
        // Calculate heal rate new each time to keep responsive
        float total = 0;
        int count = 0;
        foreach (var envNeed in beet.EnvironmentNeeds)
        {
            float needValue = beet.GetEnvironmentNeedValue(envNeed);
            float envValue = model.GetEnvironmentValue(envNeed);
            float diff = Mathf.Abs(needValue - envValue);
            float score = 1 - diff * 2;
            total += score;
            count++;
        }

        float healRate = total / count;
        beet.Health += (deltaTime / beet.LifeSpan) * healRate;
    }

    // Seconds since UTC epoch
    public static long CurrentSeconds()
    {
        return ((DateTime.UtcNow.Ticks - 621355968000000000) / TimeSpan.TicksPerSecond);
    }
}