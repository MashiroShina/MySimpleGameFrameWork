using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : ManagerBase
{
    private const int DefaultCapacity = int.MaxValue;
    private const float DefaultExpireTime = float.MaxValue;
    private Dictionary<string, IObjectPool> m_ObjectPools;

    public override int Priority
    {
        get { return 90; }
    }

    public int Count
    {
        get { return m_ObjectPools.Count; }
    }

    public ObjectPoolManager()
    {
        m_ObjectPools=new Dictionary<string, IObjectPool>();
    }

    public override void Init()
    {
     //   throw new System.NotImplementedException();
    }

    public override void Shutdown()
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.Shutdown();
        }
        m_ObjectPools.Clear();

    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.Update(elapseSeconds, realElapseSeconds);
        }
    }
    /// <summary>
    /// 检查对象池
    /// </summary>
    public bool HasObjectPool<T>() where T : ObjectBase
    {
        return m_ObjectPools.ContainsKey(typeof(T).FullName);
    }

    public ObjectPool<T> CreateObjectPool<T>(int capacity = DefaultCapacity
        , float exprireTime = DefaultExpireTime, bool allowMultiSpawn = false) where T : ObjectBase
    {
        string name = typeof(T).FullName;
        if (HasObjectPool<T>())
        {
            Debug.LogError("The object pool to be created already exists");
            return null;
        }
        ObjectPool<T> objectPool=new ObjectPool<T>(name,capacity,exprireTime,allowMultiSpawn);
        m_ObjectPools.Add(name,objectPool);
        return objectPool;
    }

    public ObjectPool<T> GetObjectPool<T>() where T : ObjectBase
    {
        IObjectPool objectPool = null;
        m_ObjectPools.TryGetValue(typeof(T).FullName, out objectPool);
        return objectPool as ObjectPool<T>;
    }

    public bool DestroyObjectPool<T>()
    {
        IObjectPool objectPool = null;
        if (m_ObjectPools.TryGetValue(typeof(T).FullName, out objectPool))
        {
            objectPool.Shutdown();
            return m_ObjectPools.Remove(typeof(T).FullName);
        }

        return false;
    }

    /// <summary>
    /// Frees releasable objects from all object pools.
    /// </summary>
    public void Release()
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.Release();
        }
    }

    /// <summary>
    /// Free unused objects from all object pools.
    /// </summary>
    public void ReleaseAllUnused()
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.ReleaseAllUnused();
        }
    }

}
