using UnityEngine;
using UnityEngine.Pool;

public interface IPoolableGo<T> where T: MonoBehaviour,IPoolableGo<T>
{
    public ObjectPool<T> ParentPool { set; }
    public void Activate(Vector3 position, Quaternion rotation);
    public void Deactivate();
}
