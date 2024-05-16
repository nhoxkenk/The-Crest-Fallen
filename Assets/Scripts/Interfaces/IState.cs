using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Tick()
    {
        // per-frame logic, include condition to transition to a new state
    }
    public void Exit()
    {
        // code that runs when we exit the state
    }
}
