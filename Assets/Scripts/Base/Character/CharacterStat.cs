using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Header("Stats")]
    public int endurance = 10;
    public float currentStamina;
    public float maxStamina;

    public int CalculateStaninaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;
        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }
}
