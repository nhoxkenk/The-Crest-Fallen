using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStaminaListener : MonoBehaviour
{
    [SerializeField] private float staminaPercent = 100;
    [SerializeField] private FloatEventChannel floatEventChannel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            DecreaseStamina(20);
        }
    }

    public void DecreaseStamina(int amount)
    {
        staminaPercent -= amount / staminaPercent;
        PublishStaminaPercent();
    }

    public void PublishStaminaPercent()
    {
        if(floatEventChannel != null)
        {
            floatEventChannel.Invoke(staminaPercent);
        }
    }
}
