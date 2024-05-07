using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolManager<T> : MonoBehaviour where T : MonoBehaviour, IPoolableGo<T>
{ 
    public GameObject poolableObjectPrefab;
    protected ObjectPool<T> Pool;
    protected int PoolSize = 20;
    protected virtual void Start()
    {
        Pool = new ObjectPool<T>(CreateObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, PoolSize, 40);
    }

    private T CreateObject()
    {
        var go = Instantiate(poolableObjectPrefab, new Vector3(0,-100,0), Quaternion.identity);
        var poolObject = go.GetComponent<T>();
        poolObject.ParentPool = Pool;
        return poolObject;
    }

    protected abstract void OnTakeFromPool(T go);

    protected abstract void OnReturnedToPool(T go);

    protected abstract void OnDestroyPoolObject(T go);
}