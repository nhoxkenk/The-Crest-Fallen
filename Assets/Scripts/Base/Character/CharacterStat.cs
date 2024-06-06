using System;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    private CharacterManager characterManager;

    [Header("Stamina Stat")]
    [SerializeField] private int endurance = 10;
    private int preEndurance = 0;

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

    [SerializeField] private float currentStamina;
    [SerializeField] private float preCurrentStamina;
    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            preCurrentStamina = currentStamina;
            currentStamina = value;
            if(preCurrentStamina > currentStamina)
            {
                DrainingStamina?.Invoke(maxStamina, currentStamina);
            } 
        }
    }
    public float maxStamina;

    [Header("Health Stat")]
    [SerializeField] private int vitality = 10;
    private int preVitality = 0;
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

    [SerializeField] private float currentHealth;
    [SerializeField] private float preCurrentHealth;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            preCurrentHealth = currentHealth;
            currentHealth = value;
            if(preCurrentHealth != currentHealth)
            {
                CurrentHealthChange?.Invoke(maxHealth, currentHealth);
            }
        }
    }
    public float maxHealth;

    public event Action<int, int> IncreaseVitalityStat;
    public event Action<int, int> IncreaseEnduranceStat;

    public event Action<float, float> DrainingStamina;
    public event Action<float, float> RegeneratingStamina;

    public event Action<float, float> CurrentHealthChange;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    //Test the function
    protected virtual void Update()
    {
        //Test function on Editor
        //CurrentHealthChange?.Invoke(maxHealth, currentHealth);

        if (Input.GetKeyDown(KeyCode.H))
        {
            Vitality += 10;
        }
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

    public virtual void OnRegeneratingStamina()
    {
        RegeneratingStamina?.Invoke(maxStamina, currentStamina);
    }

    public virtual void HandleCurrentHealthChange(float currentHealthValue, float newHealthValue)
    {
        if(currentHealth <= 0 && characterManager.IsAlive)
        {
            StartCoroutine(characterManager.ProcessDeathEvent());
            preCurrentHealth = currentHealth;
        }

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            preCurrentHealth = currentHealth;
        }
    }
}
