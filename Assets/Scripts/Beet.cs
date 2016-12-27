using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[RequireComponent(typeof(EventSubscriber))]
public class Beet : MonoBehaviour
{
    // With needs met perfectly this is how long this beet takes to heal (in ms)
    public int lifeSpan;
    public Color endColor;
    public Color selectedColor;
    public NeedRange[] needs;

    public bool IsHealed { get { return health == lifeSpan; } }

    // Health ranges from 0 - lifespan
    // 0 = dead, lifespan = cured
    private int health = 0;
    // Ranges from -1 to +1
    private float healRate;

    private bool selected = false;
    private Color startColor;
    private new Renderer renderer;

    private void Start()
    {
        health = Random.Range(10, 40);
        renderer = GetComponentInChildren<Renderer>();
        startColor = renderer.material.color;
    }

    public void MarkSelected()
    {
        selected = true;
        renderer.material.color = selectedColor;
    }

    public void MarkUnselected()
    {
        selected = false;
    }

    private void UpdateSimulation(int deltaMillis)
    {
        health += Mathf.RoundToInt(healRate * deltaMillis);
        health = Mathf.Clamp(health, 0, lifeSpan);
        float progress = (float)health / lifeSpan;
        if (!selected)
            renderer.material.color = Color.Lerp(startColor, endColor, progress);
    }

    public void SetNeedsMet(Dictionary<Need, float> needsMet)
    {
        float total = 0;
        foreach (var nr in needs)
        {
            float need = nr.Value;
            float met = needsMet.ContainsKey(nr.Need) ? needsMet[nr.Need] : 0;
            float diff = Math.Abs(need - met);
            float score = 1 - (diff / 100) * 2;
            total += score;
        }
        healRate = total / needs.Length;
        // print("Heal Rate: " + healRate);
    }

    public void RemoveFromContainer()
    {
        healRate = 0;
        var parent = this.transform.parent;
        while(parent!=null)
        {
            var container = parent.GetComponent<BeetContainer>(); ;
            if (container != null && container.Beet == this)
            {
                container.RemoveBeet();
                return;
            }
        }
    }
}

[Serializable]
public class NeedRange
{
    public Need Need;
    [Range(0, 100)]
    public float min, max;
    public float Value
    {
        get
        {
            if (value < 0)
                value = Random.Range(min, max);
            return value;
        }
    }
    private float value = -1;
}

