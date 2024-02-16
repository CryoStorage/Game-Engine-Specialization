using UnityEngine;
using System;
using System.Collections.Generic;


public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, State<EState>> States = new Dictionary<EState, State<EState>>();
    protected State<EState> CurrentState;

    private void Start()
    {
        CurrentState.EnterState();
    }
    
    private void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();
        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
            
        }
        else
        {
            ChangeState(nextStateKey);
        }
    }

    private void ChangeState(EState stateKey)
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

