﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EventSubscriber), typeof(BeetModel))]
public class Beet : MonoBehaviour
{
    public bool IsHealed { get { return Mathf.Approximately(model.health, model.lifeSpan); } }

    private bool selected = false;    
    private new Renderer renderer;
    private BeetModel model;

    private void Awake()
    {
        model = GetComponent<BeetModel>();
        renderer = GetComponentInChildren<Renderer>();
        model.startColor = renderer.material.color;
    }

    public void MarkSelected()
    {
        selected = true;
        renderer.material.color = model.selectedColor;
    }

    public void MarkUnselected()
    {
        selected = false;
    }

    private void UpdateSimulation(float deltaMillis)
    {
        model.health += (model.healRate * deltaMillis) / model.lifeSpan;
        model.health = Mathf.Clamp01(model.health);
        float progress = model.health;
        if (!selected && renderer != null)
            renderer.material.color = Color.Lerp(model.startColor, model.endColor, progress);
    }

    public void SetNeedsMet(Dictionary<Need, float> needsMet)
    {
        float total = 0;
        foreach (var nr in model.needs)
        {
            float need = nr.Value;
            float met = needsMet.ContainsKey(nr.Need) ? needsMet[nr.Need] : 0;
            float diff = Math.Abs(need - met);
            float score = 1 - diff * 2;
            total += score;
        }
        model.healRate = total / model.needs.Length;
        // print("Heal Rate: " + healRate);
    }

    public void RemoveFromContainer()
    {
        model.healRate = 0;
        var parent = this.transform.parent;
        while(parent!=null)
        {
            var container = parent.GetComponent<BeetContainer>();
            if (container != null && container.Beet == this)
            {
                container.RemoveBeet();
                return;
            }
        }
    }
}