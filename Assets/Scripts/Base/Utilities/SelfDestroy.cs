using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float timeUntilDestroy = 5;
    public CountdownTimer timer;

    private void Awake()
    {
        timer = new CountdownTimer(timeUntilDestroy);
        timer.Start();
    }

    private void Update()
    {
        if(timer.IsFinished())
        {
            Destroy(gameObject);
        }
        else
        {
            timer.Tick(Time.deltaTime);
        }
    }
}
