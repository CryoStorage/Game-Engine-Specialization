using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Interactable>(out var interactable)) return;
        interactable.Interact();
    }
}
