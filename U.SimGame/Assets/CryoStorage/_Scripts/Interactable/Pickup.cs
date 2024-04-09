using UnityEngine;

public class Pickup : Interactable
{
    [Header("Pickup Settings")]
    [SerializeField] private int value = 1;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onItemPickup;
    [Header("GameVariables")]
    [SerializeField] private IntVariableSo intCurrentGoldSo;
    
    public override void Interact()
    {
        intCurrentGoldSo.value += value;
        gameObject.SetActive(false);
        onItemPickup.Raise();
    }
}
