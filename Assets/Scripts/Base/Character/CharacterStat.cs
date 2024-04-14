using System;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Header("Stats")]
    public int endurance = 10;
    public float currentStamina;
    public float maxStamina;

    public event Action<float, float> DrainingStamina;
    public event Action<float, float> RegeneratingStamina;

    public int CalculateStaninaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    public virtual void OnDrainStaminaBasedOnAction(int stamina, bool isContinuous)
    {
        currentStamina -= isContinuous ? stamina * Time.deltaTime : stamina;
        DrainingStamina?.Invoke(maxStamina, currentStamina);
    }

    public virtual void OnRegeneratingStamina()
    {
        RegeneratingStamina?.Invoke(maxStamina, currentStamina);
    }
}
