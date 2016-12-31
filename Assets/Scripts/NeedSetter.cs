using UnityEngine;
using System;

public class NeedSetter : MonoBehaviour
{
    public Need need;
    public SliderView slider;

    public Action<Need, float> OnNeedSet;

	void Awake()
    {
        slider.OnValueChanged.AddListener((SliderView s) => { if (OnNeedSet != null) OnNeedSet(need, s.Value); });
    }
}
