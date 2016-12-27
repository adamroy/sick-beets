using UnityEngine;
using System;

public class NeedSetter : MonoBehaviour
{
    public Need need;
    public Slider slider;

    public Action<Need, float> OnNeedSet;

	void Start()
    {
        slider.OnValueChanged += (Slider s) => { if (OnNeedSet != null) OnNeedSet(need, s.Value * 100); };
    }
}
