using UnityEngine;

public class Merchant : Interactable
{
    [SerializeField] private GameEvent onBeginTrade;

    public override void Interact()
    {
        onBeginTrade.Raise();
    }
}
