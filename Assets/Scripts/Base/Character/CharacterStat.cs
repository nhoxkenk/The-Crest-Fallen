using System;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Header("Stamina Stat")]
    [SerializeField] private int endurance = 10;
    [SerializeField] private int preEndurance = 0;
    public int Endurance
    {
        get
        {
            return endurance;
        }
        set
        {
            preEndurance = endurance;
            endurance = value;
            IncreaseEnduranceStat?.Invoke(preEndurance, endurance);
        }
    }

    
    public float currentStamina;
    public float maxStamina;

    [Header("Health Stat")]
    [SerializeField] private int vitality = 10;
    [SerializeField] private int preVitality = 0;
    public int Vitality
    {
        get
        {
            return vitality;
        }
        set
        {
            preVitality = vitality;
            vitality = value;
            IncreaseVitalityStat?.Invoke(preVitality, vitality);
        }
    }

    public float currentHealth;
    public float maxHealth;

    public event Action<int, int> IncreaseVitalityStat;
    public event Action<int, int> IncreaseEnduranceStat;

    public event Action<float, float> DrainingStamina;
    public event Action<float, float> RegeneratingStamina;

    public event Action<float, float> DecreaseHealth;
    public event Action<float, float> IncreaseHealth;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    //Test the function
    protected virtual void Update()
    {
        //Test function
        //OnDecreaseHealth();
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    Vitality += 10;
        //}
    }

    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = vitality * 15;
        return Mathf.RoundToInt(health);
    }

    public int CalculateStaninaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
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

    public virtual void OnDecreaseHealth()
    {
        DecreaseHealth?.Invoke(maxHealth, currentHealth);
    }

    public virtual void OnIncreaseHealth()
    {
        IncreaseHealth?.Invoke(maxHealth, currentHealth);
    }
}
