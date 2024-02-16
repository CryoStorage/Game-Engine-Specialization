using UnityEngine;
using System;
using Unity.VisualScripting;

public abstract class State<EState> where EState : Enum
{
    public State(EState key)
    {
        StateKey = key;
    }

    public EState StateKey;
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}

