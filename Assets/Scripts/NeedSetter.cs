using UnityEngine;
using System;

public class NeedSetter : MonoBehaviour
{
    public Need need;
    public Slider slider;

    public Action<Need, float> OnNeedSet;

	void Awake()
    {
        slider.OnValueChanged += (Slider s) => { if (OnNeedSet != null) OnNeedSet(need, s.Value); };
    }
}
