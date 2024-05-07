using UnityEngine;
using UnityEngine.Pool;

[SelectionBase]
public class Missile : Projectile
{
    
    [Header("Missile Settings")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private LayerMask explosionLayerMask;
    [Header("GameEvents")]
    [SerializeField] private GameEvent onMissileExploded;
    
    private float _elapsedTime;
    private Rigidbody _rb;
    private bool _isDeactivated;
    
    private void OnEnable()
    {
        _isDeactivated = false;
        _elapsedTime = 0;
    }
    private void Update()
    {
        if (_isDeactivated) return;
        var speedMultiplier = speedCurve.Evaluate(_elapsedTime / lifeTime);
        var currentSpeed = baseSpeed * speedMultiplier;

        transform.position += transform.forward * (currentSpeed * Time.deltaTime);

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= lifeTime)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isDeactivated) return;
        var colliders =  Physics.OverlapSphere(transform.position, explosionRadius, explosionLayerMask);
        if (colliders.Length == 0)
        {
            onMissileExploded.Raise();
            Deactivate();
            return;
        }
        
        foreach (var col in colliders)
        {
            if (!col.gameObject.TryGetComponent<IAttackable>(out var attackable)) continue;
            // var inverseDistance = 1 - Vector3.Distance(transform.position, col.transform.position) / explosionRadius;
            attackable.TakeDamage(damage);
        }
        onMissileExploded.Raise();
        Deactivate();
    }

    public override void Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _isDeactivated = false;
    }

    public override void Deactivate()
    {
        if (_isDeactivated) return;
        ParentPool.Release(this);
        _isDeactivated = true;
    }
}
