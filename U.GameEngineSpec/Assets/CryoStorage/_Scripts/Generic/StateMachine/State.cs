using UnityEngine;
using System;
using Unity.VisualScripting;

public abstract class State<TEState> where TEState : Enum
{
    protected State(TEState key)
    {
        StateKey = key;
    }

    public TEState StateKey;
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract TEState GetNextState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}

