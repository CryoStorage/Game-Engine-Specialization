using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField] float interactionRadius = 2f;
    private SphereCollider _collider;

    protected virtual void OnEnable()
    {
        if(_collider != null)return;
        _collider = gameObject.AddComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = interactionRadius;

    }
    public abstract void Interact();

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
