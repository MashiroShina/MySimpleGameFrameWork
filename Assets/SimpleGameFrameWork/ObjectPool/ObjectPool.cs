using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// release object Fliter method
/// </summary>
/// <typeparam name="T">ObjectBase type</typeparam>
/// <param name="candidateObjects">Object set to be filtered</param>
/// <param name="toReleaseCount">Count of objects to be released</param>
/// <param name="expireTime">Object expiration reference time</param>
/// <returns></returns>
public delegate LinkedList<T> ReleaseObjectFilterCallback<T>(LinkedList<T> candidateObjects, int toReleaseCount,
    DateTime expireTime) where T : ObjectBase;
public class ObjectPool<T> : IObjectPool where T : ObjectBase
{
    /// <summary>
    /// objectpool Capacity
    /// </summary>
    private int m_Capacity;
    /// <summary>
    /// objectPool Object Expire Time
    /// </summary>
    private float m_ExpireTime;
    public string Name { get;private set; }
    /// <summary>
    /// objectPool linkedList
    /// </summary>
    private LinkedList<ObjectBase> m_Objects;

    public Type ObjecType
    {
        get { return typeof(T); }
    }

    public int Count
    {
        get { return m_Objects.Count; }
    }
    /// <summary>
    /// Can pool objects be repeatedly acquired? 
    /// </summary>
    public bool AllowMultiSpawn { get; private set; }

    public int CanReleaseCount
    {
        get { return GetCanReleaseObjects().Count; }
    }
    public float AutoReleaseTime { get; private set; }
    public float AutoReleaseInterval { get; set; }
    /// <summary>
    /// set or get ObjectPool capacity
    /// </summary>
    public int Capacity
    {
        get { return m_Capacity; }
        set {
            if (value<0)
            {
                Debug.LogError("Set ObjectPool Capacity <0,can't set");
            }

            if (m_Capacity==value)
            {
                return;
            }

            m_Capacity = value;
        }
    }

    public float ExpireTime
    {
        get { return m_ExpireTime; }
        set {
            if (value<0)
            {Debug.LogError("Set Object expireTime<0,can't set");
            }

            if (m_ExpireTime==value)
            {
                return;
            }

            m_ExpireTime = value;
        }
    }

    public ObjectPool(string name,int capacity,float expireTime,bool allowMultiSpawn)
    {
        Name = name;
        m_Objects=new LinkedList<ObjectBase>();
        Capacity = capacity;
        AutoReleaseInterval = expireTime;
        ExpireTime = expireTime;
        AllowMultiSpawn = allowMultiSpawn;
    }
    /// <summary>
    /// Check Object
    /// </summary>
    /// <param name="name">object Name</param>
    /// <returns>Whether the object to be checked exists</returns>
    public bool CanSpawn(string name)
    {
        foreach (ObjectBase obj in m_Objects)
        {
            if (obj.Name!=name)
            {
                continue;
            }
            //isInUse ==SpawnCount>0(Whether the object is in use)
            if (AllowMultiSpawn||!obj.IsInUse)
            {
                return true;
            }
        }

        return false;
    }

    public void Regiser(T obj,bool spwaned=false)
    {
        if (obj==null)
        {
            Debug.LogError("The object to be placed in the object pool is empty");
            return;
        }

        if (spwaned)
        {
            obj.SpawnCount++;
        }

        m_Objects.AddLast(obj);
    }

    public T Spawn(string name="")
    {
        foreach (ObjectBase obj in m_Objects)
        {
            if (obj.Name!=name)
            {
                continue;
            }

            if (AllowMultiSpawn||!obj.IsInUse)
            {
                Debug.Log("get Obj+" + typeof(T).FullName + "/" + obj.Name);
                return obj.Spawn() as T;
            }
        }

        return null;
    }

    public void Unspawn(ObjectBase obj)
    {
        Unspawn(obj.Target);
    }

    public void Unspawn(object target)
    {
        if (target == null)
        {
            Debug.LogError("The object to be recycled is empty" + typeof(object).FullName);
        }

        foreach (ObjectBase obj in m_Objects)
        {
            if (obj.Target==target)
            {
                obj.Unspawn();
                Debug.Log("The object was recycled"+typeof(T).FullName+"/"+obj.Name);
                return;
            }
        }
        Debug.LogError("Can't find recycled object"+typeof(object).FullName);
    }

    private LinkedList<T> GetCanReleaseObjects()
    {
        LinkedList<T> canReleaseObjects=new LinkedList<T>();
        foreach (ObjectBase obj in m_Objects)
        {
            if (obj.IsInUse)//obj IsInUse representative the obj to being use ,so can't Release;
            {
                continue;
            }

            canReleaseObjects.AddLast(obj as T);
        }

        return canReleaseObjects;
    }
    ///<summary>
    ///release the releasable object in the object pool.(Update excute interval =AutoReleaseTime)
    ///</summary>
    ///<param name = "toReleaseCount" > attempts to release the number of objects </param>
    ///<param name = "releaseObjectFilterCallback" > release object filter method</param>
    public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
    {
        AutoReleaseTime = 0;
        if (toReleaseCount<0)
        {
            return;
        }

        //Calculate object expiration reference time
        DateTime expireTime =DateTime.MinValue;
        if (m_ExpireTime<float.MaxValue)
        {
            //expiration reference time=current time - number of expired seconds   
            expireTime = DateTime.Now.AddSeconds(-m_ExpireTime);
            Debug.Log(" expireTime :="+ expireTime );
        }
        //get the releasable object and the object to be released.
        LinkedList<T> canReleaseObjects = GetCanReleaseObjects();
        LinkedList<T> toReleaseObjects = releaseObjectFilterCallback(canReleaseObjects, toReleaseCount, expireTime);
        if (toReleaseObjects==null||toReleaseObjects.Count<0)
        {
            return;
        }
        foreach (ObjectBase toReleaseObject in toReleaseObjects)
        {
            if (toReleaseObject==null)
            {
                Debug.LogError("Can't release Object");
            }

            foreach (ObjectBase obj in m_Objects)
            {
                if (obj !=toReleaseObject)
                {
                    continue;
                }

                m_Objects.Remove(obj);
                obj.Release();
                Debug.Log("Obj is being Release"+obj.Name);
                break;
            }
        }
    }
    /// <summary>
    /// get toReleaseObjects LinkedList<T>
    /// Default release object filtering method (not used and expired object)
    /// </summary>
    /// <param name="candidateObjects"></param>
    /// <param name="toReleaseCount"></param>
    /// <param name="expireTime"></param>
    /// <returns></returns>
    private LinkedList<T> DefaultReleaseObjectFilterCallBack(LinkedList<T> candidateObjects, int toReleaseCount,
        DateTime expireTime)
    {
        LinkedList<T> toReleaseObjects=new LinkedList<T>();
        if (expireTime>DateTime.MinValue)
        {
            LinkedListNode<T> current = candidateObjects.First;
            while (current!=null)
            {
                if (current.Value.LastUseTime<=expireTime)
                {
                    Debug.Log(current.Value.LastUseTime + " =last<---->expire= " + expireTime);
                    toReleaseObjects.AddLast(current.Value);
                    LinkedListNode<T> next = current.Next;
                    candidateObjects.Remove(current);
                    toReleaseCount--;
                    if (toReleaseCount <= 0)
                    {
                        return toReleaseObjects;
                    }
                    current = next;
                    continue;
                }
                current = current.Next;
            }
        }

        return toReleaseObjects;
    }

    public void Release()
    {
        
        Release(m_Objects.Count-m_Capacity,DefaultReleaseObjectFilterCallBack);
    }

    public void Release(int toReleaseCount)
    {
        Release(toReleaseCount,DefaultReleaseObjectFilterCallBack);
    }

    public void ReleaseAllUnused()
    {
        LinkedListNode<ObjectBase> current = m_Objects.First;
        while (current!=null)
        {
            if (current.Value.IsInUse)
            {
                current = current.Next;
                continue;
            }

            LinkedListNode<ObjectBase> next = current.Next;
            m_Objects.Remove(current);
            current.Value.Release();
            Debug.Log("Obj is being Release" + current.Value.Name);
            current = next;
        }
    }

    public void Update(float elapseSeconds, float realElapseSeconds)
    {
        AutoReleaseTime += realElapseSeconds;
        if (AutoReleaseTime<=AutoReleaseInterval)
        {
            return;
        }
        Release();
    }

    public void Shutdown()
    {
        LinkedListNode<ObjectBase> current = m_Objects.First;
        while (current!=null)
        {
            LinkedListNode<ObjectBase> next = current.Next;
            m_Objects.Remove(current);
            current.Value.Release();
            Debug.Log("Obj is be Release"+current.Value.Name);
            current = next;
        }
    }
}
