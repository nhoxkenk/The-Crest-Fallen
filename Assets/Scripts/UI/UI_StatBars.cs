using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBars : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public virtual void SetStat(int value)
    {
        slider.value = value;
    }

    public virtual void SetMaxStat(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }
}
