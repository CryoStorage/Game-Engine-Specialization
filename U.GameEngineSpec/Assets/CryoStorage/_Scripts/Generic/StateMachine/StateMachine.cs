using UnityEngine;
using System;
using System.Collections.Generic;


public abstract class StateMachine<TEState> : MonoBehaviour where TEState : Enum
{
    protected Dictionary<TEState, State<TEState>> States = new Dictionary<TEState, State<TEState>>();
    protected State<TEState> CurrentState;

    private void Start()
    {
        CurrentState.EnterState();
    }
    
    private void Update()
    {
        TEState nextStateKey = CurrentState.GetNextState();
        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
            
        }
        else
        {
            ChangeState(nextStateKey);
        }
    }

    private void ChangeState(TEState stateKey)
    {
        CurrentState.ExitState();
        CurrentState = States[stateKey];
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
}

