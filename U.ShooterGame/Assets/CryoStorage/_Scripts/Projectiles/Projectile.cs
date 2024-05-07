using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour, IPoolableGo<Projectile>
{
   public float BaseSpeed => baseSpeed;
   public int damage = 1;
   [SerializeField] protected float baseSpeed = 10f;
   [SerializeField] protected AnimationCurve speedCurve;
   [SerializeField] protected float lifeTime = 5f;


   public ObjectPool<Projectile> ParentPool { get; set; }
   public abstract void Activate(Vector3 position, Quaternion rotation);
   public abstract void Deactivate();
}
