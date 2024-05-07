using UnityEngine;
using UnityEngine.Pool;

[SelectionBase]
public class Bullet : Projectile
{
    [Header("Bullet Settings")]

    private float _elapsedTime;
    private bool _isDeactivated;

    private void OnEnable()
    {
        _isDeactivated = false;
        _elapsedTime = 0;
    }

    private void Update()
    {
        if (_isDeactivated) return;

        transform.position += transform.forward * (baseSpeed * Time.deltaTime);
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= lifeTime)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isDeactivated) return;

        if (other.gameObject.TryGetComponent<IAttackable>(out var attackable))
        {
            attackable.TakeDamage(damage);
        }

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